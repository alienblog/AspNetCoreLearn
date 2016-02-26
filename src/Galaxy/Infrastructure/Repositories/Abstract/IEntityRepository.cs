﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Galaxy.Entities;

namespace Galaxy.Infrastructure.Repositories.Abstract
{
	public interface IEntityRepository<T> where T : class, IEntity, new()
	{
		IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
		Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
		IEnumerable<T> GetAll();
		Task<IEnumerable<T>> GetAllAsync();
		T GetSingle(int id);
		T GetSingle(Expression<Func<T, bool>> predicate);
		T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
		Task<T> GetSingleAsync(int id);
		IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
		Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
		void Add(T entity);
		void Delete(T entity);
		void Edit(T entity);
		void Commit();
	}
}
