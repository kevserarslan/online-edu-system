// OnlineEduSystem.Application/DTOs/CreateApplicationUserDto.cs
using OnlineEduSystem.Domain.Enums;

namespace OnlineEduSystem.Application.DTOs
{
    public class CreateUserDto
    {
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public UserRole Role { get; set; }

        // Kullanıcıdan gelen düz metin şifre
        public string Password { get; set; } = null!;
    }
}
