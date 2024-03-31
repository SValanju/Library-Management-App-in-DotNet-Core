using System.ComponentModel.DataAnnotations;

namespace Library_WebAPI.DTOs
{
    public class UserDTO
    {
        public int id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string contact_no { get; set; }

        public int role { get; set; }

        public string user_name { get; set; }

        public string password { get; set; }

        public bool? isActive { get; set; }

        public DateTime? created_at { get; set; }

        public int created_by { get; set; }

        public DateTime? updated_at { get; set; }

        public int updated_by { get; set; }


        public string strRole { get; set; }
    }
}
