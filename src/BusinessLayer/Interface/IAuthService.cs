using ModelLayer;

namespace BusinessLayer
{
    public interface IAuthService
    {
        int Register(UserRegisterDTO userRegisterDto);
        string Login(UserLoginDTO userLoginDto);
        string LoginWithGoogle(GoogleLoginDTO googleLoginDto);
    }
}