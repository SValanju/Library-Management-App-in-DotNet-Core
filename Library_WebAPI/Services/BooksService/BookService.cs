using AutoMapper;
using Library_WebAPI.DTOs;
using Library_WebAPI.DTOs.Pagination;
using Library_WebAPI.DTOs.Requests.Books;
using Library_WebAPI.DTOs.Responses;
using Library_WebAPI.Helpers.Utils;
using Library_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Library_WebAPI.Services.BooksService
{
    public class BookService : IBookService
    {
        private readonly DotNetCoreWebApiContext dbContext;
        private readonly IMapper mapper;

        public BookService(IMapper mapper, DotNetCoreWebApiContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<BaseResponse> BooksList()
        {
            try
            {
                List<BooksDTO> books = new List<BooksDTO>();

                var query = await dbContext.TblBooks.Where(bd => bd.IsActive == true).ToListAsync();
                if (query != null && query.Any())
                {
                    query.ForEach(bk => books.Add(mapper.Map<BooksDTO>(bk)));

                    return IGlobalService.GetResponse(HttpStatusCode.OK, new { books = books });
                }
                else
                {
                    return IGlobalService.GetResponse(HttpStatusCode.NotFound, new MessageDTO("No records found!"));
                }
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }

        public async Task<BaseResponse> GetBookDetailsById(int bookId)
        {
            try
            {
                BooksDTO? book = null;

                using (dbContext)
                {
                    var filteredData = await dbContext.TblBooks.FindAsync(bookId);

                    if (filteredData != null)
                    {
                        book = new BooksDTO();
                        book = mapper.Map<BooksDTO>(filteredData);
                    }
                }

                if (book != null)
                    return IGlobalService.GetResponse(HttpStatusCode.OK, book);
                else
                    return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("Book Not Found."));
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }

        public async Task<BaseResponse> AddBook(CreateBookRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Request cannot be null.");

                TblBook bookDetails = new TblBook();
                bookDetails.Title = request.title;
                bookDetails.Description = request.description;
                bookDetails.BookCount = request.books_count;
                bookDetails.CreatedBy = request.created_by;
                bookDetails.IsActive = request.isActive;

                using (dbContext)
                {
                    dbContext.TblBooks.Add(bookDetails);
                    await dbContext.SaveChangesAsync();
                }

                return IGlobalService.GetResponse(HttpStatusCode.OK, new MessageDTO("Successfully added the new book."));
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

        public async Task<BaseResponse> UpdateBook(int id, UpdateBookRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Request cannot be null.");

                var filteredData = await dbContext.TblBooks.FindAsync(id);

                if (filteredData != null)
                {
                    filteredData.BookId = request.id;
                    filteredData.Title = request.title;
                    filteredData.Description = request.description;
                    filteredData.BookCount = request.books_count;
                    filteredData.UpdatedAt = request.updated_at;
                    filteredData.UpdatedBy = request.updated_by;
                    filteredData.IsActive = request.isActive;

                    await dbContext.SaveChangesAsync();

                    return IGlobalService.GetResponse(HttpStatusCode.OK, new MessageDTO("Book details updated successfully."));
                }

                return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("Book not found."));
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

        public async Task<BaseResponse> DeleteBook(int id, DeleteBookRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var filteredData = await dbContext.TblBooks.FindAsync(id);

                if (filteredData != null)
                {
                    filteredData.BookId = request.id;
                    filteredData.IsActive = request.isActive;
                    filteredData.DeletedAt = request.deleted_at;
                    filteredData.DeletedBy = request.deleted_by;

                    await dbContext.SaveChangesAsync();

                    return IGlobalService.GetResponse(HttpStatusCode.OK, new MessageDTO("Book deleted successfully."));
                }

                return IGlobalService.GetResponse(HttpStatusCode.BadRequest, new MessageDTO("Book not found."));
            }
            catch (Exception ex)
            {
                return IGlobalService.GetErrorResponse(ex);
            }
        }
    }
}
