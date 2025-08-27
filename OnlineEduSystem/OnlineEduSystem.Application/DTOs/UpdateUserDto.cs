// OnlineEduSystem.Application/DTOs/UpdateApplicationUserDto.cs
using OnlineEduSystem.Domain.Enums;

namespace OnlineEduSystem.Application.DTOs
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public UserRole? Role { get; set; }
        public string? NewPassword { get; set; }
    }
}
