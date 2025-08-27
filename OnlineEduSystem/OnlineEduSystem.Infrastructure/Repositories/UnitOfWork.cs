
using OnlineEduSystem.Domain.Interfaces;
using OnlineEduSystem.Infrastructure.Data;
using System.Threading.Tasks;

namespace OnlineEduSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Courses = new CourseRepository(_context);
            Enrollments = new EnrollmentRepository(_context);
            Users = new UserRepository(_context);
            CourseMaterials = new CourseMaterialRepository(_context);
        }

        public ICourseRepository Courses { get; private set; }
        public IEnrollmentRepository Enrollments { get; private set; }
        public IUserRepository Users { get; private set; }

        // Yeni eklenen property
        public ICourseMaterialRepository CourseMaterials { get; private set; }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
