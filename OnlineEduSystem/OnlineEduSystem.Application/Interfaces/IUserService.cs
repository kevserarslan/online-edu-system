// OnlineEduSystem.Application/Interfaces/IUserService.cs
using OnlineEduSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<UserDto?> CreateAsync(CreateUserDto dto);
        Task<List<UserDto>> GetInstructorsAsync();
        Task<UserDto?> UpdateAsync(string id, UpdateUserDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
