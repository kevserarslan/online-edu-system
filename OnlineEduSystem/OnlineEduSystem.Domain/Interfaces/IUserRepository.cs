using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser, string>
    {
        // Login için kullanıcı çekme
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
        // Eğitmen rollü kullanıcıları çekme
        Task<IEnumerable<ApplicationUser>> GetInstructorsAsync();
    }
}
