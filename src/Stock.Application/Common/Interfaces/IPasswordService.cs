namespace Stock.Application.Common.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        string GenerateRandomPassword(int length = 10);
    }
} 