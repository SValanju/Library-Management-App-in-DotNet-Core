using DataEncryption;
using Library_WebApp.Models.Books;
using Library_WebApp.Services.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net;

namespace Library_WebApp.Controllers
{
    [Authorize(Policy = "ManagerRole")]
    public class BooksController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly IConfiguration config;

        public BooksController(ILogger<HomeController> logger, IBookService bookService, IConfiguration config)
        {
            _logger = logger;
            _bookService = bookService;
            this.config = config;
        }


        public IActionResult BooksListing()
        {
            ViewBag.AES_Key = new HtmlString(config.GetValue<string>("AES:Key"));
            ViewBag.AES_IV = new HtmlString(config.GetValue<string>("AES:IV"));
            HttpContext.Session.Remove("booksData");
            return View();
        }

        public async Task<IActionResult> LoadData()
        {
            dynamic data = await _bookService.GetList();
            return Json(data);
        }

        public async Task<IActionResult> ManageBook([FromQuery] string bookId)
        {
            BooksManagement model = new BooksManagement();
            if (!string.IsNullOrWhiteSpace(bookId))
            {
                model = await _bookService.GetBook(bookId);
                model.btnSubmitLabel = "Update Book";
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageBook(BooksManagement formData)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.AddUpdateBook(formData);
                return Json(response);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
            return Json(new { status = HttpStatusCode.BadRequest, errorList = errors });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var response = await _bookService.DeleteBook(bookId);
            return Json(response);
        }
    }
}
