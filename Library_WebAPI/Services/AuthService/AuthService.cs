using AutoMapper;
using DataEncryption;
using Library_WebAPI.DTOs;
using Library_WebAPI.DTOs.Requests.Auth;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Helpers.Exceptions;
using Library_WebAPI.Helpers.Utils;
using Library_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Library_WebAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DefaultContext _context;
        private readonly IMapper mapper;

        public AuthService(DefaultContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // Token authentication
        public async Task<BaseResponse> Authenticate(AuthenticateRequest request)
        {
            try
            {
                TblUser? user = await _context.TblUsers.Where(u => u.UserName == request.username).FirstOrDefaultAsync();

                if (user == null)
                {
                    return IGlobalService.GetResponse(HttpStatusCode.Unauthorized, new MessageDTO("Invalid credentials!"));
                }

                if (user.Password == request.password)
                {
                    string userRole = await _context.TblRoles.Where(r => r.RoleId == user.UserRole).Select(r => r.RoleDesc).FirstOrDefaultAsync();

                    if (string.IsNullOrWhiteSpace(userRole))
                        throw new RoleNotFoundException("User role not found. Contact your administrator.");

                    // generate JWT
                    string jwt = JwtUtils.GenerateJwtToken(user, userRole);

                    TblLogin? loginDetails = await _context.TblLogins.Where(ld => ld.UserId == user.UserId).FirstOrDefaultAsync();

                    if (loginDetails == null)
                    {
                        loginDetails = new TblLogin();
                        loginDetails.UserId = user.UserId;
                        loginDetails.Token = jwt;
                        loginDetails.CreatedAt = DateTime.Now;
                        loginDetails.CreatedBy = user.UserId;

                        await _context.TblLogins.AddAsync(loginDetails);
                    }
                    else
                    {
                        loginDetails.Token = jwt;
                        loginDetails.UpdatedAt = DateTime.Now;
                        loginDetails.UpdatedBy = user.UserId;
                    }

                    await _context.SaveChangesAsync();

                    UserDTO userDTO = mapper.Map<UserDTO>(user);
                    userDTO.strRole = userRole;

                    return IGlobalService.GetResponse(HttpStatusCode.OK, new { token = jwt, userDetails = userDTO });
                }
                else
                {
                    return IGlobalService.GetResponse(HttpStatusCode.Unauthorized, new MessageDTO("Invalid credentials!"));
                }
            }
            catch(Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }
    }
}
