namespace Stock.Application.Common.Exceptions
{
    public class DomainException : ApplicationException
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
} 