namespace Stock.Application.Constants
{
    public static class JwtTokenSettings
    {
        public const string Audience = "JwtSettings:Audience";
        public const string Issuer = "JwtSettings:Issuer";
        public const string SecretKey = "JwtSettings:SecretKey";
        public const string TokenExpirationInMinutes = "JwtSettings:TokenExpirationInMinutes";
    }
} 