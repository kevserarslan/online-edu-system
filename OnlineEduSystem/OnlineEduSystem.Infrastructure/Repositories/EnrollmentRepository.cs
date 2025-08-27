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
    public class EnrollmentRepository
          : GenericRepository<Enrollment, int>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Enrollment>> GetAllWithDetailsAsync()
            => await _dbSet
                .Include(e => e.Course)
                .Include(e => e.User)
                .ToListAsync();

        public async Task<Enrollment?> GetByIdWithDetailsAsync(int id)
            => await _dbSet
                .Include(e => e.Course)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
    }
}
