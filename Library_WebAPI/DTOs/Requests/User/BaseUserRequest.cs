using Library_WebAPI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs.Requests.User
{
    public class BaseUserRequest
    {
        [Required]
        public string first_name { get; set; }

        public string? last_name { get; set; }

        [Required]
        [CustomEmail(ErrorMessage = "Email is not valid.")]
        public string email { get; set; }

        public string? contact_no { get; set; }

        [Required]
        [PositiveNumber]
        public int role { get; set; }

        [Required]
        public string user_name { get; set; }

        [Required]
        public string password { get; set; }
    }
}
