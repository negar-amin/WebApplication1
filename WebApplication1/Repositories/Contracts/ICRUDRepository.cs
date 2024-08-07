using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICRUDRepository<T> where T : class
{
	Task<IEnumerable<T>> GetAllAsync();
	Task<T> GetByIdAsync(int id);
	Task<T> GetByIdAsync(int pk1, int pk2);
	Task<T> AddAsync(T entity);
	Task UpdateAsync(T entity);
	Task DeleteAsync(int id);
}
