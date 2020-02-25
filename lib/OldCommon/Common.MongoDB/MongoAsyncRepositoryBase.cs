using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.ComponentModel;
using Common.Repository;
using MongoDB.Driver;

namespace Common.MongoDB
{
    /// <summary>
    /// The mongo repository base for all the repositories.
    /// </summary>
    /// <typeparam name="TEntity">The mongo entity.</typeparam>
    public abstract class MongoAsyncRepositoryBase<TEntity> : ComponentBase, 
                                                              IAsyncRepository<TEntity, string>
        where TEntity : IMongoEntity
    {
        protected IMongoCollection<TEntity> Collection => GetCollection();

        protected abstract IMongoCollection<TEntity> GetCollection();

        /// <summary>
        /// See <see cref="IAsyncEditableRepository{TEntity, TKey}.CreateAsync"/> for more information.
        /// </summary>
        /// <param name="entity">The mongo entity.</param>
        public async Task CreateAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// See <see cref="IAsyncEditableRepository{TEntity, TKey}.DeleteAsync"/> for more information.
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        public async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(x => x.Id == id);
        }

        /// <summary>
        /// See <see cref="IAsyncEditableRepository{TEntity, TKey}.UpdateAsync"/> for more information.
        /// </summary>
        /// <param name="entity">The mongo entity.</param>
        public async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }

        /// <summary>
        /// See <see cref="IAsyncReadableRepository{TEntity,TKey}.ReadAsync"/> for more information.
        /// </summary>
        /// <param name="predicate">The condition to find the entity.</param>
        /// <returns>The found entity.</returns>
        public async Task<TEntity> ReadAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return (await Collection.FindAsync(predicate)).FirstOrDefault();
        }

        /// <summary>
        /// See <see cref="IAsyncReadableRepository{TEntity,TKey}.ReadManyAsync"/> for more information.
        /// </summary>
        /// <param name="predicate">The condition to find the entities.</param>
        /// <returns>The found entities.</returns>
        public async Task<IEnumerable<TEntity>> ReadManyAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }
    }
}