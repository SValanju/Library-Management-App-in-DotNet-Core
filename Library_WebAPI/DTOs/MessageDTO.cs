namespace Library_WebAPI.DTOs
{
    public class MessageDTO
    {
        public string message { get; set; }

        public MessageDTO(string message)
        {
            this.message = message;
        }
    }
}
