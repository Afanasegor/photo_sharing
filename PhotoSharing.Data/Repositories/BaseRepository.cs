using Microsoft.EntityFrameworkCore;
using PhotoSharing.Core.Interfaces.Repositories;
using PhotoSharing.Core.Models;
using PhotoSharing.Data.Context;

namespace PhotoSharing.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;

        public BaseRepository(ApplicationContext applicationContext)
        {
            _context = applicationContext;
        }

        public virtual IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public virtual async Task<IList<T>> GetAll()
        {
            var result = await _context.Set<T>().AsNoTracking().OrderByDescending(x => x.Created).ToListAsync();
            return result;
        }

        public virtual async Task<T> GetById(Guid id)
        {
            var result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public virtual async Task<T> Create(T entity)
        {
            var now = DateTime.UtcNow;
            entity.Created = now;
            entity.Modified = now;

            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            var now = DateTime.UtcNow;
            entity.Modified = now;

            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Delete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
