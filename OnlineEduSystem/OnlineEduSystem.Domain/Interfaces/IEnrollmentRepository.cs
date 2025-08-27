using OnlineEduSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment, int>
    {
        // Gerekirse özel metotlar buraya
        Task<IEnumerable<Enrollment>> GetAllWithDetailsAsync();
        Task<Enrollment?> GetByIdWithDetailsAsync(int id);
    }
}
