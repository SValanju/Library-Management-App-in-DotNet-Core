using Library_WebApp.Helpers;
using Library_WebApp.Models.User;
using System.Data;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Collections;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.Xml;
using DataEncryption;
using Microsoft.AspNetCore.Html;
using Library_WebApp.Services.ConsumeAPI;
using Newtonsoft.Json.Linq;
using Library_WebApp.Models;
using Microsoft.AspNetCore.Http;

namespace Library_WebApp.Services.User
{
    public class UserService : IUserService
    {
        private string apiUrl;
        private string endpoint;
        private readonly IConsumeAPI api;

        public UserService(IConfiguration configuration, IConsumeAPI consumeAPI)
        {
            apiUrl = configuration.GetValue<string>("apiUrl");
            api = consumeAPI;
        }

        public async Task<LoginViewResult> GetToken(LoginViewModel login)
        {
            LoginViewResult result = new LoginViewResult();
            string password = Supports.DecryptAES(login.password);

            endpoint = apiUrl + "/Auth/login";
            string requestBody = JsonSerializer.Serialize(new
            {
                username = login.user_name,
                password = Supports.GenerateMD5(password)
            });

            // Calling API service
            var response = await api.PostAsync(endpoint, requestBody);
            var content = JObject.Parse(response);

            if (content.ContainsKey("status_code"))
            {
                int status = (int)content["status_code"];

                if(content["data"] != null && content["data"] is JObject objData)
                {
                    if (status == 200)
                    {
                        result.token = objData["token"].ToString();
                        if(objData["userDetails"] !=null && objData["userDetails"] is JObject objUser)
                        {
                            result.userDetails = new UserData
                            {
                                id = Convert.ToInt32(objUser["id"]),
                                first_name = objUser["first_name"].ToString(),
                                last_name = objUser["last_name"].ToString(),
                                email = objUser["email"].ToString(),
                                contact_no = objUser["contact_no"].ToString(),
                                role = Convert.ToInt32(objUser["role"]),
                                user_name = objUser["user_name"].ToString(),
                                isActive = Convert.ToBoolean(objUser["isActive"]),
                                strRole = objUser["strRole"].ToString()
                            };
                        }
                    }
                    else if (objData.ContainsKey("message"))
                    {
                        result.errors = objData["message"].ToString();
                    }
                }
            }
            else if(content.ContainsKey("status"))
            {
                string message = $"Error code {(int)content["status"]}...<br>";

                if (content.ContainsKey("errors"))
                {
                    JObject errors = (JObject)content["errors"];

                    // Iterate through each error
                    foreach (var error in errors.Properties())
                    {
                        message += $"{char.ToUpper(error.Name[0]) + error.Name.Substring(1)}: {error.Value.First}<br>";
                    }
                }
                result.errors = message;
            }
            else
            {
                result.errors = "Something went wrong! Please try again later...";
            }
            return result;
        }
    }
}
