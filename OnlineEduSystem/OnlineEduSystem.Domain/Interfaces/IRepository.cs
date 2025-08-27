using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEduSystem.Domain.Interfaces
{
    // Temel interface (ID bazlı olmayan operasyonlar)
    public interface IRepository<T> where T : class
    {
        // IQueryable
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }

    // ID tipini generic olarak alan interface
    public interface IRepository<T, TId> : IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(TId id);
    }

}
