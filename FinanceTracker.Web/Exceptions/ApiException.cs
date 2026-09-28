using System.Net;

namespace FinanceTracker.Web.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public Dictionary<string, string[]> ValidationErrors { get; }

        public ApiException(string message,
                            HttpStatusCode statusCode,
                            Dictionary<string, string[]>? validationErrors = null) 
                            :base(message)
        {
            StatusCode = statusCode;
            ValidationErrors = validationErrors ?? [];
        }
    }
}
