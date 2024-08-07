using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CRUDRepository<T> : ICRUDRepository<T> where T : class
{
	protected readonly ApplicationDbContext _context;
	private readonly DbSet<T> _dbSet;

	public CRUDRepository(ApplicationDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<T>();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _dbSet.ToListAsync();
	}

	public async Task<T> GetByIdAsync(int id)
	{
		return await _dbSet.FindAsync(id);
	}

	public async Task<T> AddAsync(T entity)
	{
		await _dbSet.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task UpdateAsync(T entity)
	{
		_dbSet.Attach(entity);
		_context.Entry(entity).State = EntityState.Modified;
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var entity = await _dbSet.FindAsync(id);
		if (entity != null)
		{
			_dbSet.Remove(entity);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<T> GetByIdAsync(int pk1, int pk2)
	{
		return await _dbSet.FindAsync(pk1, pk2); ;
	}
}
