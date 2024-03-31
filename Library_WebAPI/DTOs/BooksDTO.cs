namespace Library_WebAPI.DTOs
{
    public class BooksDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int books_count { get; set; }
        public bool? isActive { get; set; }
        public DateTime? created_at { get; set; }
        public int created_by { get; set; }
        public DateTime? updated_at { get; set; }
        public int updated_by { get; set; }
    }
}
