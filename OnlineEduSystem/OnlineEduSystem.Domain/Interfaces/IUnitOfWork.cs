using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OnlineEduSystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; } // 
        IUserRepository Users { get; } // 
        ICourseMaterialRepository CourseMaterials { get; }
        Task<int> SaveChangesAsync();
    }
}
