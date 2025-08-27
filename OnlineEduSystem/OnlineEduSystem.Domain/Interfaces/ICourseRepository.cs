using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Interfaces
{
    public interface ICourseRepository : IRepository<Course, int>
    {
        Task<IEnumerable<Course>> GetAllWithDetailsAsync();
        Task<Course?> GetByIdWithDetailsAsync(int id);
    }
}
