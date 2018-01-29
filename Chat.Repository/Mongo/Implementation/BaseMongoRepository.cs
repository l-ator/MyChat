using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Chat.Infrastructure.Model;
using Chat.Repository.Mongo.Interface;

namespace Chat.Repository.Mongo.Implementation
{
	public class BaseMongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IEntity
	{
		public virtual IMongoCollection<TEntity> Collection => Collection;

		public virtual async Task<TEntity> GetByIdAsync(string id) => await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();

		public virtual async Task<TEntity> FindAsync(FilterDefinition<TEntity> filter)
		{
			if (filter == null)
				filter = Builders<TEntity>.Filter.Empty;

			return await (await Collection.FindAsync(filter)).FirstOrDefaultAsync();
		}

		public virtual async Task<TEntity> FindAsync(
			Expression<Func<TEntity, bool>> predicate)
		{
			if (predicate == null)
				predicate = x => true;

			return await (await Collection.FindAsync(predicate)).FirstOrDefaultAsync();
		}

		public virtual async Task<ICollection<TEntity>> FindAllAsync(FilterDefinition<TEntity> filter)
		{
			if (filter == null)
				filter = Builders<TEntity>.Filter.Empty;

			return await (await Collection.FindAsync(filter)).ToListAsync();
		}

		public virtual async Task<ICollection<TEntity>> FindAllAsync(
			Expression<Func<TEntity, bool>> predicate = null)
		{
			if (predicate == null)
				predicate = x => true;

			return await (await Collection.FindAsync(predicate)).ToListAsync();
		}

		public virtual async Task InsertAsync(TEntity entity) => await Collection.InsertOneAsync(entity);
		public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, InsertManyOptions options = null, CancellationToken cancellationToken = default(CancellationToken)) => await Collection.InsertManyAsync(entities);
		public virtual async Task<ReplaceOneResult> ReplaceAsync(FilterDefinition<TEntity> filter, TEntity entity, UpdateOptions options) => await Collection.ReplaceOneAsync(filter, entity, options);
		public virtual async Task<ReplaceOneResult> ReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity entity, UpdateOptions options) => await Collection.ReplaceOneAsync(filter, entity, options);
		public virtual async Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken)) => await Collection.DeleteOneAsync(filter, options, cancellationToken);
		public virtual async Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken)) => await Collection.DeleteOneAsync(filter, options, cancellationToken);
		public virtual async Task<DeleteResult> DeleteManyAsync(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken)) => await Collection.DeleteManyAsync(filter, options, cancellationToken);
		public virtual async Task<DeleteResult> DeleteManyAsync(Expression<Func<TEntity, bool>> filter, DeleteOptions options, CancellationToken cancellationToken = default(CancellationToken)) => await Collection.DeleteManyAsync(filter, options, cancellationToken);

		public virtual void Insert(TEntity entity) => Collection.InsertOne(entity);
		public virtual ICollection<TEntity> FindAll(FilterDefinition<TEntity> filter) => Collection.Find(filter ?? Builders<TEntity>.Filter.Where(x => true)).ToList();
		public virtual ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> filter) => Collection.Find(filter ?? (x => true)).ToList();
	}
}
