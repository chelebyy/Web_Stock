namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Represents the data transfer object for a user login request.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the user's registration number (sicil).
        /// </summary>
        /// <example>12345</example>
        public string Sicil { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        /// <example>P@ssw0rd123</example>
        public string Password { get; set; } = string.Empty;
    }
} 