using Library_WebAPI.DTOs.Requests.Auth;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace Library_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        // constructor
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        // endpoints
        [HttpPost("login")]
        public async Task<BaseResponse> Authenticate([FromBody] AuthenticateRequest request)
        {
            return await authService.Authenticate(request);
        }
    }
}
