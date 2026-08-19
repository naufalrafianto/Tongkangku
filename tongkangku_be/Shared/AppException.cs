using System.Net;

namespace tongkangku_be.Shared
{
    public class AppException:Exception
    {
        public HttpStatusCode StatusCode {  get; }
        public string ErrorCode { get; }
        public object? Details{ get; }

        public AppException(string message, HttpStatusCode statusCode, string errorCode ="BAD_REQUEST", object? detail = null):base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Details = detail;
        }        
    }

    public class NotFoundException: AppException
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound, "NOT_FOUND") { }
    }
    
    public class ValidationException: AppException
    {
        public ValidationException(object errors): base("Validation failed", HttpStatusCode.BadRequest, "VALIDATION_ERROR", errors) { }
    }
}
