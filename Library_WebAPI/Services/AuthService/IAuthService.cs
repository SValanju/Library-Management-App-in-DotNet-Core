using Library_WebAPI.DTOs.Requests.Auth;
using Library_WebAPI.DTOs.Responses;

namespace Library_WebAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<BaseResponse> Authenticate(AuthenticateRequest request);
    }
}
