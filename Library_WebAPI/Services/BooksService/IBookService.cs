using Library_WebAPI.DTOs.Requests.Books;
using Library_WebAPI.DTOs.Responses;

namespace Library_WebAPI.Services.BooksService
{
    public interface IBookService
    {
        Task<BaseResponse> BooksList();

        Task<BaseResponse> GetBookDetailsById(int bookId);

        Task<BaseResponse> AddBook(CreateBookRequest request);

        Task<BaseResponse> UpdateBook(int id, UpdateBookRequest request);

        Task<BaseResponse> DeleteBook(int id, DeleteBookRequest request);
    }
}
