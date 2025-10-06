using System;
namespace AuthenticationService.Core.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task InsertAsync(T entity);

		Task DeleteAsync(string id);

		void DeleteRangeAsync(IEnumerable<T> entities);

		void Update(T item);
	}
}

