using Library_WebAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs.Requests.User
{
    public class CreateUserRequest : BaseUserRequest
    {
        [Required]
        [PositiveNumber]
        public int created_by { get; set; }

        public bool isActive { get; set; }
    }
}
