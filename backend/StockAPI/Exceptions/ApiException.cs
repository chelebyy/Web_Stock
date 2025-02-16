using System.Net;

namespace StockAPI.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string UserMessage { get; }
        public object? AdditionalData { get; }

        public ApiException(
            string message,
            string userMessage,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            object? additionalData = null)
            : base(message)
        {
            StatusCode = statusCode;
            UserMessage = userMessage;
            AdditionalData = additionalData;
        }
    }
}
