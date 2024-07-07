using microservice.product.Data.Entities;

namespace microservice.product.Data.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAsync(IEnumerable<long>? Ids);
        Task<long> AddAsync(T value);
        Task<int> UpdateAsync(T value);
        Task DeleteAsync(long id);
    }
}
