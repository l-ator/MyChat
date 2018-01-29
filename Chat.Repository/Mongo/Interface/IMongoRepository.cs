using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Chat.Infrastructure.Model;

namespace Chat.Repository.Mongo.Interface
{
	public interface IMongoRepository<TEntity> where TEntity : IEntity
	{
		IMongoCollection<TEntity> Collection { get; }

		Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
		Task<TEntity> FindAsync(FilterDefinition<TEntity> filter);
		Task<ICollection<TEntity>> FindAllAsync(FilterDefinition<TEntity> filter);
		Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter);
		Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
		Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
		Task<DeleteResult> DeleteManyAsync(Expression<Func<TEntity, bool>> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
		Task<DeleteResult> DeleteManyAsync(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
		Task<TEntity> GetByIdAsync(string id);
		Task InsertAsync(TEntity entity);
		Task InsertManyAsync(IEnumerable<TEntity> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken));
		Task<ReplaceOneResult> ReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity entity, UpdateOptions options);
		Task<ReplaceOneResult> ReplaceAsync(FilterDefinition<TEntity> filter, TEntity entity, UpdateOptions options);

		ICollection<TEntity> FindAll(FilterDefinition<TEntity> filter);
		ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> filter);
		void Insert(TEntity entity);
	}

//	#region generic
//	public interface IMongoRepository<TEntity, TKey> where TEntity : IEntity<TKey>
//	{
//		IMongoCollection<TEntity> Collection { get; }
				
//		Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
//		Task<TEntity> FindAsync(FilterDefinition<TEntity> filter);
//		Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
//		Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
//		Task<DeleteResult> DeleteManyAsync(Expression<Func<TEntity, bool>> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
//		Task<DeleteResult> DeleteManyAsync(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken));
//		Task<TEntity> GetByIdAsync(string id);
//		Task InsertAsync(TEntity entity);
//		Task InsertManyAsync(IEnumerable<TEntity> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken));
//		Task<ReplaceOneResult> ReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity entity, UpdateOptions options);
//		Task<ReplaceOneResult> ReplaceAsync(FilterDefinition<TEntity> filter, TEntity entity, UpdateOptions options);

//		void Insert(TEntity entity);
//	}
//#endregion
}