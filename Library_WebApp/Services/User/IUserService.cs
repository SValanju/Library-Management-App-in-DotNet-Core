using Library_WebApp.Models.User;

namespace Library_WebApp.Services.User
{
    public interface IUserService
    {
        Task<LoginViewResult> GetToken(LoginViewModel login);
    }
}
