using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using QuantityMeasurementWebApi.Data;

namespace BusinessLayer
{
    public class AuthService : IAuthService
    {
        private readonly QuantityDbContext _context;
        private readonly JwtSettingsDTO _jwtSettings;

        public AuthService(QuantityDbContext context, JwtSettingsDTO jwtSettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
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
                Role = string.IsNullOrWhiteSpace(userRegisterDto.Role)
                    ? "User"
                    : userRegisterDto.Role.Trim()
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
            if (user is null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            {
                throw new QuantityMeasurementException("Invalid email or password.");
            }

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
