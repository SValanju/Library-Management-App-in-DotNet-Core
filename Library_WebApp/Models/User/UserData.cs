namespace Library_WebApp.Models.User
{
    public class UserData
    {
        public int id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string contact_no { get; set; }

        public int role { get; set; }

        public string user_name { get; set; }

        public string password { get; set; }

        public bool isActive { get; set; }


        public string strRole { get; set; }
        public string full_name => $"{first_name} {last_name}".Trim();
    }
}
