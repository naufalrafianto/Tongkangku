using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tongkangku_be.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public ApiErrorResponse? Error { get; set; }
    
        public static ApiResponse<T> SuccessResult(T data, string message = "Request successful") {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Error = null
            };
        }

        public static ApiResponse<T> ErrorResult(string message, string errorCode, object? details = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Error = new ApiErrorResponse
                {
                    Code = errorCode,
                    Details = details
                }
            };
        }
    }

    public class ApiErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}
