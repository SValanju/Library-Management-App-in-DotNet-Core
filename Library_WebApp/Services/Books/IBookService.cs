using Library_WebApp.Models.Books;

namespace Library_WebApp.Services.Books
{
    public interface IBookService
    {        
        Task<dynamic> GetList();

        Task<BooksManagement> GetBook(string id);

        Task<BooksManagementResult> AddUpdateBook(BooksManagement book);

        Task<BooksManagementResult> DeleteBook(int id);
    }
}
