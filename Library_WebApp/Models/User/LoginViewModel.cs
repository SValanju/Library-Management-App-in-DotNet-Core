using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library_WebApp.Models.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name is required!")]
        [DisplayName("User Name")]
        public string user_name { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class LoginViewResult
    {
        public string token { get; set; }
        public string errors { get; set; }
        public UserData userDetails { get; set; }
    }
}
