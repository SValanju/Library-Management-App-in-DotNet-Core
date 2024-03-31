using Library_WebAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs.Requests.Books
{
    public class CreateBookRequest : BaseBooksRequest
    {
        [Required]
        [PositiveNumber]
        public int created_by { get; set; }

        public bool isActive { get; set; }
    }
}
