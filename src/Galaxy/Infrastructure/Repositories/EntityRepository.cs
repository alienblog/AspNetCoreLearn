using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Galaxy.Entities;
using Galaxy.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Galaxy.Infrastructure.Repositories
{
	public class EntityRepository<T> : IEntityRepository<T>
			where T : class, IEntity, new()
	{

		private readonly GalaxyContext _context;

		#region Properties
		public EntityRepository(GalaxyContext context)
		{
			_context = context;
		}
		#endregion
		public virtual IEnumerable<T> GetAll()
		{
			return _context.Set<T>().AsEnumerable();
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}
		public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = _context.Set<T>();
			query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
			return query.AsEnumerable();
		}

		public virtual async Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = _context.Set<T>();
			query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
			return await query.ToListAsync();
		}
		public T GetSingle(int id)
		{
			return _context.Set<T>().FirstOrDefault(x => x.Id == id);
		}

		public T GetSingle(Expression<Func<T, bool>> predicate)
		{
			return _context.Set<T>().FirstOrDefault(predicate);
		}

		public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = _context.Set<T>();
			query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

			return query.Where(predicate).FirstOrDefault();
		}

		public async Task<T> GetSingleAsync(int id)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
		}
		public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			return _context.Set<T>().Where(predicate);
		}

		public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().Where(predicate).ToListAsync();
		}

		public virtual void Add(T entity)
		{
			_context.Entry(entity);
			_context.Set<T>().Add(entity);
		}

		public virtual void Edit(T entity)
		{
			EntityEntry dbEntityEntry = _context.Entry(entity);
			dbEntityEntry.State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			EntityEntry dbEntityEntry = _context.Entry(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}

		public virtual void Commit()
		{
			_context.SaveChanges();
		}
	}
}
