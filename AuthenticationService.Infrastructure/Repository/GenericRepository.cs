using System;
using AuthenticationService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
        private readonly AuthenticationServiceDbContext _context;
        private readonly DbSet<T> _db;

		public GenericRepository(AuthenticationServiceDbContext context)
		{
            _context = context;
            _db = _context.Set<T>();
		}

        /// <summary>
        /// Deletes an Object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            _db.Remove(await _db.FindAsync(id));
        }

        /// <summary>
        /// Deletes List of Objects
        /// </summary>
        /// <param name="entities"></param>
        public async void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        /// <summary>
        /// Inserts an Object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(T entity)
        {
            try
            {
                await _db.AddAsync(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(T item)
        {
            //attaches instance to the context, then sets the state as modified
            _db.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}

