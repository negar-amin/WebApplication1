namespace WebApplication1.Repositories
{
	public interface IJoinTableRepository<T> where T : class
	{
		Task<T> GetByPrimaryKey(int pk1 , int pk2);
		Task<ICollection<T>> GetAll();
		Task UpdateRecord(T entity);
	}
}
