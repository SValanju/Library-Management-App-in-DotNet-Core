using Library_WebApp.Helpers.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library_WebApp.Models.Books
{
    public class BooksManagement
    {
        public int? id { get; set; }

        [Required(ErrorMessage = "Please enter title.")]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Description")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Please enter books count.")]
        [PositiveNumber]
        [Display(Name = "Books Count")]
        public int books_count { get; set; }

        [Display(Name = "Availability")]
        public bool isActive { get; set; } = true;

        public DateTime? created_at { get; set; }

        public int created_by { get; set; }

        public DateTime? updated_at { get; set; }

        public int updated_by { get; set; }



        public string btnSubmitLabel { get; set; } = "Add Book";
        public string? errorMessage { get; set; }
    }

    public class BooksManagementResult
    {
        public int status { get; set; }
        public string data { get; set; }
        public string error { get; set; }
        public BooksManagement bookDetails { get; set; }
    }
}
