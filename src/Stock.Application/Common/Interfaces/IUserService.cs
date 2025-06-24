using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Kullanıcı yönetimi işlemleri için arayüz.
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserRoleAsync(int userId, int roleId);
        // DTO kullanan versiyonlar da eklenebilir
        // Task<IEnumerable<UserDto>> GetUsersDtoAsync();
        // Task<UserDto?> GetUserDtoByIdAsync(int id);
        // Task<UserDto> CreateUserDtoAsync(CreateUserDto userDto);
        // Task<UserDto> UpdateUserDtoAsync(int id, UpdateUserDto userDto);
    }
} 