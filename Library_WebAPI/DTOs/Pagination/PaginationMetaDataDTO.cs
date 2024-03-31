namespace Library_WebAPI.DTOs.Pagination
{
    public class PaginationMetaDataDTO<T>
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public T Items { get; set; }
    }
}
