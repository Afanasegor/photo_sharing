using PhotoSharing.Core.Models;

namespace PhotoSharing.Core.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get();

        Task<T> GetById(Guid id);
        Task<IList<T>> GetAll();

        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
