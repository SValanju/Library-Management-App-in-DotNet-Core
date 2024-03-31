using Library_WebAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs.Requests.User
{
    public class UpdateUserRequest : BaseUserRequest
    {
        [Required]
        [PositiveNumber]
        public int id { get; set; }

        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        [Required]
        [PositiveNumber]
        public int updated_by { get; set; }

        public bool isActive { get; set; }
    }
}
