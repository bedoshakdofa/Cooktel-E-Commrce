namespace Cooktel_E_commrece.Middleware
{
    public class APIException
    {
        public string Message { get; set; }

        public string Detail { get; set; }

        public int? StatusCode { get; set; }

        public APIException(string message, string ?detail, int statusCode)
        {
            Message = message;
            Detail = detail;
            StatusCode = statusCode;
        }
    }


}
