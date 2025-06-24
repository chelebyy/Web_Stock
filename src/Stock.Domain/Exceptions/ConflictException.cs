using System;

namespace Stock.Domain.Exceptions
{
    /// <summary>
    /// Kaynak çakışması durumunda (örneğin, aynı isimde başka bir kayıt zaten var) fırlatılacak özel istisna sınıfı.
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException() : base()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 