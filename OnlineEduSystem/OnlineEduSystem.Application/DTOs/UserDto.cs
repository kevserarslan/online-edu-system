// OnlineEduSystem.Application/DTOs/ApplicationUserDto.cs
using OnlineEduSystem.Domain.Enums;

namespace OnlineEduSystem.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}
