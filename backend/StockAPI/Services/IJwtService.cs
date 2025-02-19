using StockAPI.Models;

namespace StockAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
} 