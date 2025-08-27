// OnlineEduSystem.Infrastructure/Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Enums;
using OnlineEduSystem.Domain.Interfaces;
using OnlineEduSystem.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEduSystem.Infrastructure.Repositories
{
    public class UserRepository
        : GenericRepository<ApplicationUser, string>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<ApplicationUser>> GetInstructorsAsync()
        {
            return await _dbSet
                .Where(u => u.Role == UserRole.Instructor)
                .ToListAsync();
        }
    }
}
