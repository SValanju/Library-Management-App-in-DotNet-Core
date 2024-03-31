using Library_WebAPI.DTOs.Requests.User;
using Library_WebAPI.DTOs.Responses;

namespace Library_WebAPI.Services.UserService
{
    public interface IUserService
    {
        Task<BaseResponse> UsersList();

        Task<BaseResponse> GetUserById(int id);

        Task<BaseResponse> CreateUser(CreateUserRequest request);

        Task<BaseResponse> UpdateUser(int id, UpdateUserRequest request);

        Task<BaseResponse> DeleteUser(int id, DeleteUserRequest request);
    }
}
