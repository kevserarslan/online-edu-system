using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Interfaces
{
    public interface ICourseMaterialRepository : IRepository<CourseMaterial, int>
    {
        // Gerekirse özel metotlar
        Task<IEnumerable<CourseMaterial>> GetByCourseIdAsync(int courseId);

    }
   
}
