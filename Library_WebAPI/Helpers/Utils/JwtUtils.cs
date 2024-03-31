using Library_WebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_WebAPI.Helpers.Utils
{
    public static class JwtUtils
    {
        static string secret = AppSettingsHelper.GetSetting("JWT:Key");

        public static string GenerateJwtToken(TblUser user, string UserRole)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secret);

            // token claims
            List<Claim> claims = new List<Claim>
            {
                new Claim("user_id", user.UserId.ToString()),
                new Claim("username", user.UserName),
                new Claim(ClaimTypes.Role, UserRole)
            };

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }

        public static bool ValidateJwtToken(string jwt)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(secret);

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);
                JwtSecurityToken validatedJWT = (JwtSecurityToken)validatedToken;

                // Get claims
                int userId = int.Parse(validatedJWT.Claims.First(claim => claim.Type == "user_id").Value);

                using (DotNetCoreWebApiContext dbContext = new DotNetCoreWebApiContext())
                {
                    TblUser? user = dbContext.TblUsers.Find(userId);

                    if (user == null)
                    {
                        return false;
                    }
                    else
                    {
                        // Check if the given token is the last issued token to the user
                        TblLogin loginDetails = dbContext.TblLogins.Where(ld => ld.UserId == userId).FirstOrDefault();

                        if (loginDetails.Token != jwt)
                        {
                            return false;
                        }
                        else
                        {
                            // Token is valid and is latest
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
