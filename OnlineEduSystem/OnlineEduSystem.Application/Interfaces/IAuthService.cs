using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(CreateUserDto dto);
        Task<AuthResult> LoginAsync(LoginDto dto);
    }
}
