using Azure.Core;
using DataEncryption;
using Library_WebAPI.Services.BooksService;
using Library_WebApp.Models.Books;
using Library_WebApp.Models.User;
using Library_WebApp.Services.ConsumeAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Buffers;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library_WebApp.Services.Books
{
    public class BookService : IBookService
    {
        private string apiUrl;
        private string endpoint;
        private readonly IConsumeAPI api;

        private readonly IHttpContextAccessor httpContextAccessor;

        public BookService(IConfiguration configuration, IConsumeAPI consumeAPI, IHttpContextAccessor httpContextAccessor)
        {
            apiUrl = configuration.GetValue<string>("apiUrl");
            api = consumeAPI;
            this.httpContextAccessor = httpContextAccessor;
        }


        public async Task<dynamic> GetList()
        {
            dynamic result = null;
            LoginViewResult loggedInUser = JsonConvert.DeserializeObject<LoginViewResult>(httpContextAccessor.HttpContext.Session.GetString("loggedInUser"));

            int recordsTotal = 0;
            var draw = httpContextAccessor.HttpContext.Request.Form["draw"].FirstOrDefault();

            // Skip number of Rows
            var start = httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32(start) : 0;

            //Paging Size (10, 20, 50,100)
            var length = httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;

            // Sort Column Direction (asc, desc)
            var sortColumnDirection = httpContextAccessor.HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();

            // Sort Column Name
            var sortColumn = httpContextAccessor.HttpContext.Request.Form["columns[" + httpContextAccessor.HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();

            // Search Value from (Search box)
            var searchValue = httpContextAccessor.HttpContext.Request.Form["search[value]"].FirstOrDefault();

            string dataFromDB = string.Empty;
            List<BooksManagement> bookList = null;
            if (httpContextAccessor.HttpContext.Session.GetString("booksData") != null)
            {
                // getting all books data from session
                dataFromDB = httpContextAccessor.HttpContext.Session.GetString("booksData");
                bookList = JsonConvert.DeserializeObject<List<BooksManagement>>(dataFromDB);
            }
            else
            {
                // getting all books data from api
                endpoint = apiUrl + "/Books/getbooks";

                // Calling API service
                var response = await api.GetAsync(endpoint, loggedInUser.token);
                var content = JObject.Parse(response);

                if (content.ContainsKey("status_code"))
                {
                    int status = (int)content["status_code"];
                    var data = content["data"];

                    if (data != null && data is JObject objData)
                    {
                        if (status == 200)
                        {
                            if (objData.ContainsKey("books"))
                            {
                                dataFromDB = objData["books"].ToString();
                                httpContextAccessor.HttpContext.Session.SetString("booksData", dataFromDB);
                                bookList = JsonConvert.DeserializeObject<List<BooksManagement>>(dataFromDB);
                            }
                        }
                        else
                        {
                            if (objData.ContainsKey("message"))
                            {
                                result = new
                                {
                                    status = status,
                                    error = data["message"].ToString()
                                };
                            }
                        }
                    }
                }
                else if (content.ContainsKey("status"))
                {
                    int status = (int)content["status"];
                    string message = $"Error code {status}...<br>";

                    if (content.ContainsKey("errors"))
                    {
                        JObject errors = (JObject)content["errors"];

                        // Iterate through each error
                        foreach (var error in errors.Properties())
                        {
                            message += $"{char.ToUpper(error.Name[0]) + error.Name.Substring(1)}: {error.Value.First}<br>";
                        }
                    }
                    else if (content.ContainsKey("message"))
                    {
                        message += content["message"].ToString();
                    }
                    result = new
                    {
                        status = status,
                        error = message
                    };
                }
                else
                {
                    result = new
                    {
                        status = HttpStatusCode.InternalServerError,
                        error = "Something went wrong! Please try again later..."
                    };
                }
            }

            if (result is null)
            {
                //total number of rows counts
                recordsTotal = bookList.Count;

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //bookList = bookList.OrderBy(sortColumn + " " + sortColumnDirection);
                    var prop = typeof(BooksManagement).GetProperty(sortColumn);
                    if (prop != null)
                    {
                        if (sortColumnDirection == "asc")
                            bookList = bookList.OrderBy(b => prop.GetValue(b, null)).ToList();
                        else
                            bookList = bookList.OrderByDescending(b => prop.GetValue(b, null)).ToList();
                    }
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    bookList = bookList.Where(b =>
                        b.GetType().GetProperties().Any(prop =>
                            prop.GetValue(b, null)?.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ?? false
                        )).ToList();
                }

                //Paging
                bookList = bookList.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data
                result = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = bookList };
            }

            return result;
        }

        public async Task<BooksManagementResult> AddUpdateBook(BooksManagement book)
        {
            BooksManagementResult result = null;

            if (book.id > 0)
                result = await UpdateBook(book);
            else
                result = await AddBook(book);

            return result;
        }

        private async Task<BooksManagementResult> AddBook(BooksManagement book)
        {
            BooksManagementResult result = new BooksManagementResult();
            LoginViewResult loggedInUser = JsonConvert.DeserializeObject<LoginViewResult>(httpContextAccessor.HttpContext.Session.GetString("loggedInUser"));

            endpoint = apiUrl + "/Books/addbook";
            string requestBody = JsonConvert.SerializeObject(new
            {
                title = book.title,
                description = book.description,
                books_count = book.books_count,
                isActive = book.isActive,
                created_by = loggedInUser.userDetails.id
            });

            // Calling API service
            var response = await api.PostAsync(endpoint, requestBody, loggedInUser.token);
            var content = JObject.Parse(response);

            if (content.ContainsKey("status_code"))
            {
                int status = (int)content["status_code"];
                var data = content["data"];

                if (data != null && data is JObject objData)
                {
                    result.status = status;

                    if (status == 200)
                    {
                        result.data = data["message"].ToString();
                    }
                    else
                    {
                        if (objData.ContainsKey("message"))
                        {
                            result.error = data["message"].ToString();
                        }
                    }
                }
            }
            else if (content.ContainsKey("status"))
            {
                string message = string.Empty;

                if (content.ContainsKey("errors"))
                {
                    JObject errors = (JObject)content["errors"];

                    // Iterate through each error
                    foreach (var error in errors.Properties())
                    {
                        message += $"{char.ToUpper(error.Name[0]) + error.Name.Substring(1)}: {error.Value.First}<br>";
                    }
                }
                else if (content.ContainsKey("message"))
                {
                    message = content["message"].ToString();
                }

                result.status = (int)content["status"];
                result.error = message;
            }
            else
            {
                result.status = (int)HttpStatusCode.InternalServerError;
                result.error = "Something went wrong! Please try again later...";
            }

            return result;
        }

        public async Task<BooksManagement> GetBook(string id)
        {
            int bookId;
            BooksManagement model = new BooksManagement();
            
            try
            {
                bookId = Convert.ToInt32(Supports.DecryptAES(id));
            }
            catch
            {
                model.errorMessage = "Corrupted ID received! Please try again.";
                return model;
            }

            BooksManagementResult resBookData = await GetBook(bookId);

            if (resBookData.status != 200)
            {
                model.errorMessage = resBookData.error;
            }
            else
            {
                model = resBookData.bookDetails;
            }

            return model;
        }

        private async Task<BooksManagementResult> GetBook(int bookId)
        {
            if (bookId <= 0)
                return new BooksManagementResult
                {
                    status = (int)HttpStatusCode.BadRequest,
                    error = "Incorrect book id received!"
                };

            BooksManagementResult result = new BooksManagementResult();
            LoginViewResult loggedInUser = JsonConvert.DeserializeObject<LoginViewResult>(httpContextAccessor.HttpContext.Session.GetString("loggedInUser"));

            endpoint = apiUrl + "/Books/getbook/" + bookId;
            // Calling API service
            var response = await api.GetAsync(endpoint, loggedInUser.token);
            var content = JObject.Parse(response);

            if (content.ContainsKey("status_code"))
            {
                int status = (int)content["status_code"];
                var data = content["data"];

                if (data != null && data is JObject objData)
                {
                    result.status = status;

                    if (status == 200)
                    {
                        result.bookDetails = new BooksManagement
                        {
                            id = !string.IsNullOrWhiteSpace(data["id"].ToString()) ? Convert.ToInt32(data["id"]) : (int?)null,
                            title = data["title"].ToString(),
                            description = data["description"].ToString(),
                            isActive = !string.IsNullOrWhiteSpace(data["isActive"].ToString()) ? Convert.ToBoolean(data["isActive"]) : false,
                            books_count = !string.IsNullOrWhiteSpace(data["books_count"].ToString()) ? Convert.ToInt32(data["books_count"]) : 0
                        };
                    }
                    else
                    {
                        if (objData.ContainsKey("message"))
                        {
                            result.error = data["message"].ToString();
                        }
                    }
                }
            }
            else if (content.ContainsKey("status"))
            {
                string message = string.Empty;

                if (content.ContainsKey("errors"))
                {
                    JObject errors = (JObject)content["errors"];

                    // Iterate through each error
                    foreach (var error in errors.Properties())
                    {
                        message += $"{char.ToUpper(error.Name[0]) + error.Name.Substring(1)}: {error.Value.First}<br>";
                    }
                }
                else if (content.ContainsKey("message"))
                {
                    message = content["message"].ToString();
                }

                result.status = (int)content["status"];
                result.error = message;
            }
            else
            {
                result.status = (int)HttpStatusCode.InternalServerError;
                result.error = "Something went wrong! Please try again later...";
            }

            return result;
        }

        private async Task<BooksManagementResult> UpdateBook(BooksManagement book)
        {
            BooksManagementResult result = new BooksManagementResult();
            LoginViewResult loggedInUser = JsonConvert.DeserializeObject<LoginViewResult>(httpContextAccessor.HttpContext.Session.GetString("loggedInUser"));

            endpoint = apiUrl + "/Books/updatebook/" + book.id;
            string requestBody = JsonConvert.SerializeObject(new
            {
                id = book.id,
                title = book.title,
                description = book.description,
                books_count = book.books_count,
                isActive = book.isActive,
                updated_by = loggedInUser.userDetails.id,
                updated_at = DateTime.Now
            });

            // Calling API service
            var response = await api.PutAsync(endpoint, requestBody, loggedInUser.token);
            var content = JObject.Parse(response);

            if (content.ContainsKey("status_code"))
            {
                int status = (int)content["status_code"];
                var data = content["data"];

                if (data != null && data is JObject objData)
                {
                    result.status = status;

                    if (status == 200)
                    {
                        result.data = data["message"].ToString();
                    }
                    else
                    {
                        if (objData.ContainsKey("message"))
                        {
                            result.error = data["message"].ToString();
                        }
                    }
                }
            }
            else if (content.ContainsKey("status"))
            {
                string message = string.Empty;

                if (content.ContainsKey("errors"))
                {
                    JObject errors = (JObject)content["errors"];

                    // Iterate through each error
                    foreach (var error in errors.Properties())
                    {
                        message += $"{char.ToUpper(error.Name[0]) + error.Name.Substring(1)}: {error.Value.First}<br>";
                    }
                }
                else if (content.ContainsKey("message"))
                {
                    message = content["message"].ToString();
                }

                result.status = (int)content["status"];
                result.error = message;
            }
            else
            {
                result.status = (int)HttpStatusCode.InternalServerError;
                result.error = "Something went wrong! Please try again later...";
            }

            return result;
        }

        public async Task<BooksManagementResult> DeleteBook(int bookId)
        {
            if (bookId <= 0)
                return new BooksManagementResult
                {
                    status = (int)HttpStatusCode.BadRequest,
                    error = "Incorrect book id received!"
                };

            BooksManagementResult result = new BooksManagementResult();
            LoginViewResult loggedInUser = JsonConvert.DeserializeObject<LoginViewResult>(httpContextAccessor.HttpContext.Session.GetString("loggedInUser"));

            endpoint = apiUrl + "/Books/deletebook/" + bookId;
            string requestBody = JsonConvert.SerializeObject(new
            {
                id = bookId,
                deleted_at = DateTime.Now,
                deleted_by = loggedInUser.userDetails.id
            });

            // Calling API service
            var response = await api.PutAsync(endpoint, requestBody, loggedInUser.token);
            var content = JObject.Parse(response);

            if (content.ContainsKey("status_code"))
            {
                int status = (int)content["status_code"];
                var data = content["data"];

                if (data != null && data is JObject objData)
                {
                    result.status = status;

                    if (status == 200)
                    {
                        result.data = data["message"].ToString();
                    }
                    else
                    {
                        if (objData.ContainsKey("message"))
                        {
                            result.error = data["message"].ToString();
                        }
                    }
                }
            }
            else if (content.ContainsKey("status"))
            {
                string message = string.Empty;

                if (content.ContainsKey("errors"))
                {
                    JObject errors = (JObject)content["errors"];

                    // Iterate through each error
                    foreach (var error in errors.Properties())
                    {
                        message += $"{char.ToUpper(error.Name[0]) + error.Name.Substring(1)}: {error.Value.First}<br>";
                    }
                }
                else if (content.ContainsKey("message"))
                {
                    message = content["message"].ToString();
                }

                result.status = (int)content["status"];
                result.error = message;
            }
            else
            {
                result.status = (int)HttpStatusCode.InternalServerError;
                result.error = "Something went wrong! Please try again later...";
            }

            return result;
        }
    }
}
