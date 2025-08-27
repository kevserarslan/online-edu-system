using Microsoft.EntityFrameworkCore;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
using OnlineEduSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Infrastructure.Repositories
{
    public class CourseMaterialRepository
        : GenericRepository<CourseMaterial, int>, ICourseMaterialRepository
    {
        public CourseMaterialRepository(AppDbContext context)
            : base(context) { }

        // Örnek: Belirli bir kursun materyallerini getiren yardımcı metot
        public async Task<IEnumerable<CourseMaterial>> GetByCourseIdAsync(int courseId)
        {
            return await _dbSet
                .Where(m => m.CourseId == courseId)
                .ToListAsync();
        }
    }
}
