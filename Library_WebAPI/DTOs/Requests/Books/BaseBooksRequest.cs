using Library_WebAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs.Requests.Books
{
    public class BaseBooksRequest
    {
        [Required]
        public string title { get; set; }

        public string description { get; set; }

        [Required]
        [PositiveNumber]
        public int books_count { get; set; }
    }
}
