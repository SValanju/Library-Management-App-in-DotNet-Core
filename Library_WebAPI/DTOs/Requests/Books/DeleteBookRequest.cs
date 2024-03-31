using Library_WebAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs.Requests.Books
{
    public class DeleteBookRequest
    {
        [Required]
        [PositiveNumber]
        public int id { get; set; }

        public readonly bool isActive = false;

        public DateTime deleted_at { get; set; } = DateTime.UtcNow;

        [Required]
        [PositiveNumber]
        public int deleted_by { get; set; }
    }
}
