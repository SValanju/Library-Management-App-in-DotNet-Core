using System.Net;

namespace Library_WebAPI.DTOs.Responses
{
    public class BaseResponse
    {
        public int status_code { get; set; } // to return the status of the response
        public object data { get; set; } = default!; // to return data associated with the response
    }
}
