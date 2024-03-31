using Library_WebAPI.DTOs.Requests.User;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Services;
using Library_WebAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        // constructor
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // endpoints
        [HttpGet("getlist")]
        public async Task<BaseResponse> UsersList()
        {
            return await userService.UsersList();
        }

        [HttpGet("get/{id}")]
        public async Task<BaseResponse> GetUserById(int id)
        {
            return await userService.GetUserById(id);
        }

        [HttpPost("adduser")]
        public async Task<BaseResponse> CreateUser([FromBody] CreateUserRequest request)
        {
            return await userService.CreateUser(request);
        }

        [HttpPut("update/{id}")]
        public async Task<BaseResponse> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            return await userService.UpdateUser(id, request);
        }

        [HttpPut("delete/{id}")]
        public async Task<BaseResponse> DeleteUser(int id, [FromBody] DeleteUserRequest request)
        {
            return await userService.DeleteUser(id, request);
        }
    }
}
