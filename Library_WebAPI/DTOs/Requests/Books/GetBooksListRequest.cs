using Library_WebAPI.Helpers.Attributes;

namespace Library_WebAPI.DTOs.Requests.Books
{
    public class GetBooksListRequest
    {
        [PositiveNumber]
        public int PageNumber { get; set; } = 1;

        [PositiveNumber]
        public int PageSize { get; set; } = 10;
    }
}
