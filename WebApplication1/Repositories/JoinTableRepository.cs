
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repositories
{
	public class JoinTableRepository <T>: IJoinTableRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		private readonly DbSet<T> _dbSet;
        public JoinTableRepository(ApplicationDbContext context)
        {
            _context = context;
			_dbSet = _context.Set<T>();
        }
        public async Task<ICollection<T>> GetAll()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T> GetByPrimaryKey(int pk1 , int pk2)
		{
			return await _dbSet.FindAsync(pk1 , pk2);
		}

		public async Task UpdateRecord(T entity)
		{
			_dbSet.Attach(entity);
			_dbSet.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
	}
}
