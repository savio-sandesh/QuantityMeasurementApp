using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using QuantityMeasurementWebApi.Data;

namespace BusinessLayer
{
    public class AuthService : IAuthService
    {
        private readonly QuantityDbContext _context;
        private readonly JwtSettingsDTO _jwtSettings;
        private readonly GoogleAuthSettingsDTO _googleAuthSettings;

        public AuthService(
            QuantityDbContext context,
            JwtSettingsDTO jwtSettings,
            GoogleAuthSettingsDTO googleAuthSettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
            _googleAuthSettings = googleAuthSettings ?? throw new ArgumentNullException(nameof(googleAuthSettings));
        }

        public int Register(UserRegisterDTO userRegisterDto)
        {
            if (string.IsNullOrWhiteSpace(userRegisterDto.FullName) ||
                string.IsNullOrWhiteSpace(userRegisterDto.Email) ||
                string.IsNullOrWhiteSpace(userRegisterDto.Password))
            {
                throw new QuantityMeasurementException("FullName, Email and Password are required.");
            }

            string email = userRegisterDto.Email.Trim().ToLowerInvariant();
            var existingUser = _context.Users.FirstOrDefault(x => x.Email == email);
            if (existingUser is not null)
            {
                throw new QuantityMeasurementException("A user with this email already exists.");
            }

            var userEntity = new UserEntity
            {
                FullName = userRegisterDto.FullName.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password),
                Role = string.IsNullOrWhiteSpace(userRegisterDto.Role) ? "User" : userRegisterDto.Role.Trim(),
                AuthProvider = "Local",
                EmailVerified = false
            };

            _context.Users.Add(userEntity);
            _context.SaveChanges();

            return userEntity.Id;
        }

        public string Login(UserLoginDTO userLoginDto)
        {
            if (string.IsNullOrWhiteSpace(userLoginDto.Email) || string.IsNullOrWhiteSpace(userLoginDto.Password))
            {
                throw new QuantityMeasurementException("Email and Password are required.");
            }

            string email = userLoginDto.Email.Trim().ToLowerInvariant();
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user is null)
            {
                throw new QuantityMeasurementException("No account found with this email. Please sign up first.");
            }

            if (string.Equals(user.AuthProvider, "Google", StringComparison.OrdinalIgnoreCase) &&
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new QuantityMeasurementException("This account uses Google sign-in. Please continue with Google.");
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            {
                throw new QuantityMeasurementException("Invalid email or password.");
            }

            return GenerateJwt(user);
        }

        public string LoginWithGoogle(GoogleLoginDTO googleLoginDto)
        {
            if (string.IsNullOrWhiteSpace(googleLoginDto.IdToken))
            {
                throw new QuantityMeasurementException("Google id token is required.");
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = GoogleJsonWebSignature.ValidateAsync(
                    googleLoginDto.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _googleAuthSettings.ClientId }
                    }).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw new QuantityMeasurementException("Invalid Google token.");
            }

            if (!payload.EmailVerified)
            {
                throw new QuantityMeasurementException("Google email is not verified.");
            }

            if (string.IsNullOrWhiteSpace(payload.Email))
            {
                throw new QuantityMeasurementException("Google account email is missing.");
            }

            string email = payload.Email.Trim().ToLowerInvariant();
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            bool shouldSave = false;

            if (user is null)
            {
                user = new UserEntity
                {
                    FullName = string.IsNullOrWhiteSpace(payload.Name) ? email : payload.Name.Trim(),
                    Email = email,
                    PasswordHash = null,
                    Role = "User",
                    AuthProvider = "Google",
                    ProviderUserId = payload.Subject,
                    EmailVerified = true
                };

                _context.Users.Add(user);
                shouldSave = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(user.ProviderUserId))
                {
                    user.ProviderUserId = payload.Subject;
                    shouldSave = true;
                }

                if (!user.EmailVerified)
                {
                    user.EmailVerified = true;
                    shouldSave = true;
                }

                if (string.IsNullOrWhiteSpace(user.AuthProvider))
                {
                    user.AuthProvider = "Google";
                    shouldSave = true;
                }
            }

            if (shouldSave)
            {
                _context.SaveChanges();
            }

            return GenerateJwt(user);
        }

        private string GenerateJwt(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}