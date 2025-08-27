// OnlineEduSystem.Infrastructure/Repositories/CourseRepository.cs
using Microsoft.EntityFrameworkCore;
using OnlineEduSystem.Domain.Entities;
using OnlineEduSystem.Domain.Interfaces;
using OnlineEduSystem.Infrastructure.Data;
using OnlineEduSystem.Infrastructure.Repositories;
namespace OnlineEduSystem.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course, int>, ICourseRepository
    {
        public CourseRepository(AppDbContext ctx) : base(ctx) { }

        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Course>()
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Course>()
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}