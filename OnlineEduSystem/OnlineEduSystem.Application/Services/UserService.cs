// OnlineEduSystem.Application/Services/UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;

namespace OnlineEduSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
            => _repo = repo;

        public async Task<List<UserDto>> GetAllAsync()
        {
            var all = await _repo.GetAllAsync();
            return all
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Role = u.Role
                })
                .ToList();
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;
            return new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Role = u.Role
            };
        }

        public async Task<UserDto?> CreateAsync(CreateUserDto dto)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = dto.FullName,
                UserName = dto.UserName,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Role = user.Role
            };
        }

        public async Task<UserDto?> UpdateAsync(string id, UpdateUserDto dto)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;

            if (!string.IsNullOrEmpty(dto.FullName))
                u.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.UserName))
                u.UserName = dto.UserName;
            if (dto.Role.HasValue)
                u.Role = dto.Role.Value;
            if (!string.IsNullOrEmpty(dto.NewPassword))
                u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _repo.Update(u);
            await _repo.SaveChangesAsync();

            return new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Role = u.Role
            };
        }
        public async Task<List<UserDto>> GetInstructorsAsync()
        {
            var list = await _repo.GetInstructorsAsync();
            return list
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Role = u.Role
                })
                .ToList();
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return false;

            _repo.Delete(u);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
