using Library_WebAPI.DTOs.Requests.Books;
using Library_WebAPI.DTOs.Requests.User;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Services.BooksService;
using Library_WebAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        // contructor
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        // endpoints
        [HttpGet("getbooks")]
        public async Task<BaseResponse> BooksList()
        {
            return await bookService.BooksList();
        }

        [HttpGet("getbook/{id}")]
        public async Task<BaseResponse> GetBookDetailsById(int id)
        {
            return await bookService.GetBookDetailsById(id);
        }

        [HttpPost("addbook")]
        public async Task<BaseResponse> CreateBook([FromBody] CreateBookRequest request)
        {
            return await bookService.AddBook(request);
        }

        [HttpPut("updatebook/{id}")]
        public async Task<BaseResponse> UpdateBook(int id, [FromBody] UpdateBookRequest request)
        {
            return await bookService.UpdateBook(id, request);
        }

        [HttpPut("deletebook/{id}")]
        public async Task<BaseResponse> DeleteBook(int id, [FromBody] DeleteBookRequest request)
        {
            return await bookService.DeleteBook(id, request);
        }
    }
}
