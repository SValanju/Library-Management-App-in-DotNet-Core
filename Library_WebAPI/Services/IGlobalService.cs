using Library_WebAPI.DTOs.Responses;
using System.Net;

namespace Library_WebAPI.Services
{
    public interface IGlobalService
    {
        public static BaseResponse GetResponse(HttpStatusCode status, Object objData)
        {
            return new BaseResponse
            {
                status_code = (int)status,
                data = objData
            };
        }

        public static BaseResponse GetErrorResponse(Exception ex, int status = StatusCodes.Status500InternalServerError)
        {
            string msg = status == 500 ? "Internal Server Error : " : string.Empty;
            return new BaseResponse
            {
                status_code = status,
                data = new { message = msg + ex.Message }
            };
        }
    }
}
