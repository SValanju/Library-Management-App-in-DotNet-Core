using AutoMapper;
using Azure;
using DataEncryption;
using Library_WebAPI.DTOs;
using Library_WebAPI.DTOs.Requests.User;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Helpers.Utils;
using Library_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Library_WebAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DotNetCoreWebApiContext dbContext;
        private readonly IMapper mapper;

        public UserService(DotNetCoreWebApiContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<BaseResponse> UsersList()
        {
            try
            {
                List<UserDTO> users = new List<UserDTO>();

                using (dbContext)
                {
                    var userList = await dbContext.TblUsers.Where(u => u.IsActive == true).ToListAsync();

                    userList.ForEach(u => users.Add(mapper.Map<UserDTO>(u)));
                    //userList.ForEach(u => users.Add(new UserDTO
                    //{
                    //    id = u.UserId,
                    //    first_name = u.FirstName,
                    //    last_name = !string.IsNullOrWhiteSpace(u.LastName) ? u.LastName : string.Empty,
                    //    email = u.EmailId,
                    //    contact_no = !string.IsNullOrWhiteSpace(u.ContactNumber) ? u.ContactNumber : string.Empty,
                    //    role = u.UserRole,
                    //    user_name = u.UserName,
                    //    password = string.Empty,
                    //    isActive = u.IsActive,
                    //    created_at = u.CreatedAt,
                    //    created_by = u.CreatedBy.HasValue ? Convert.ToInt32(u.CreatedBy) : 0,
                    //    updated_at = u.UpdatedAt,
                    //    updated_by = u.UpdatedBy.HasValue ? Convert.ToInt32(u.UpdatedBy) : 0
                    //}));
                }

                return IGlobalService.GetResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }

        public async Task<BaseResponse> GetUserById(int id)
        {
            try
            {
                UserDTO? user = null;
                using (dbContext)
                {
                    var filteredUser = await dbContext.TblUsers.FindAsync(id);

                    if (filteredUser != null)
                    {
                        user = new UserDTO();

                        user = mapper.Map<UserDTO>(filteredUser);

                        //user.id = filteredUser.UserId;
                        //user.first_name = filteredUser.FirstName;
                        //user.last_name = filteredUser.LastName;
                        //user.email = filteredUser.EmailId;
                        //user.contact_no = filteredUser.ContactNumber;
                        //user.role = filteredUser.UserRole;
                        //user.user_name = filteredUser.UserName;
                        //user.password = filteredUser.Password;
                        //user.isActive = filteredUser.IsActive;
                        //user.created_at = filteredUser.CreatedAt;
                        //user.created_by = filteredUser.CreatedBy.HasValue ? Convert.ToInt32(filteredUser.CreatedBy) : 0;
                        //user.updated_at = filteredUser.UpdatedAt;
                        //user.updated_by = filteredUser.UpdatedBy.HasValue ? Convert.ToInt32(filteredUser.UpdatedBy) : 0;                    }
                    }
                }

                if (user != null)
                    return IGlobalService.GetResponse(HttpStatusCode.OK, user);
                else
                    return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("No User Found."));
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }

        public async Task<BaseResponse> CreateUser(CreateUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Request cannot be null.");

                // check if user exists based on username
                TblUser? existingUser = await dbContext.TblUsers.Where(u => u.UserName == request.user_name).FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    // check if user exists based on email
                    existingUser = await dbContext.TblUsers.Where(u => u.EmailId == request.email).FirstOrDefaultAsync();
                    if (existingUser != null)
                    {
                        return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("Email already exists!"));
                    }
                }
                else
                {
                    return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("User name already exists!"));
                }

                // create new instance of user model
                TblUser user = new TblUser();
                user.FirstName = request.first_name;
                user.LastName = request.last_name;
                user.EmailId = request.email;
                user.ContactNumber = request.contact_no;
                user.UserRole = request.role;
                user.UserName = request.user_name;
                user.Password = Supports.GenerateMD5(request.password);
                user.IsActive = request.isActive;
                user.CreatedBy = request.created_by;

                using (dbContext)
                {
                    dbContext.TblUsers.Add(user);
                    await dbContext.SaveChangesAsync();
                }

                return IGlobalService.GetResponse(HttpStatusCode.OK, new MessageDTO("Successfully created the new user."));
            }
            catch (DbUpdateException ex)
            {
                return IGlobalService.GetErrorResponse(ex, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }

        public async Task<BaseResponse> UpdateUser(int id, UpdateUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Request cannot be null.");

                var filteredUser = await dbContext.TblUsers.FindAsync(id);

                if (filteredUser != null)
                {
                    filteredUser.UserId = request.id;
                    filteredUser.FirstName = request.first_name;
                    filteredUser.LastName = request.last_name;
                    filteredUser.EmailId = request.email;
                    filteredUser.ContactNumber = request.contact_no;
                    filteredUser.UserRole = request.role;
                    filteredUser.UserName = request.user_name;

                    if (!string.IsNullOrWhiteSpace(request.password))
                        filteredUser.Password = Supports.GenerateMD5(request.password);

                    filteredUser.IsActive = request.isActive;
                    filteredUser.UpdatedAt = request.updated_at;
                    filteredUser.UpdatedBy = request.updated_by;

                    await dbContext.SaveChangesAsync();

                    return IGlobalService.GetResponse(HttpStatusCode.OK, new MessageDTO("User updated successfully."));
                }

                return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("No user found."));
            }
            catch (DbUpdateException ex)
            {
                return IGlobalService.GetErrorResponse(ex, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }

        public async Task<BaseResponse> DeleteUser(int id, DeleteUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var filteredUser = await dbContext.TblUsers.FindAsync(id);

                if (filteredUser != null)
                {
                    filteredUser.UserId = request.id;
                    filteredUser.IsActive = request.isActive;
                    filteredUser.DeletedAt = request.deleted_at;
                    filteredUser.DeletedBy = request.deleted_by;

                    await dbContext.SaveChangesAsync();

                    return IGlobalService.GetResponse(HttpStatusCode.OK, new MessageDTO("User deleted successfully."));
                }

                return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("No user found."));
            }
            catch (DbUpdateException ex)
            {
                return IGlobalService.GetErrorResponse(ex, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }
    }
}
