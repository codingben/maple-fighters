using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.ComponentModel;
using Common.Repository;
using MongoDB.Driver;

namespace Common.MongoDB
{
    /// <summary>
    /// The mongo repository base for all the repositories.
    /// </summary>
    /// <typeparam name="TEntity">The mongo entity.</typeparam>
    public abstract class MongoRepositoryBase<TEntity> : ComponentBase,
                                                         IRepository<TEntity, string>
        where TEntity : IMongoEntity
    {
        protected IMongoCollection<TEntity> Collection => GetCollection();

        protected abstract IMongoCollection<TEntity> GetCollection();

        /// <summary>
        /// See <see cref="IAsyncEditableRepository{TEntity, TKey}.CreateAsync"/> for more information.
        /// </summary>
        /// <param name="entity">The mongo entity.</param>
        public void Create(TEntity entity)
        {
            Collection.InsertOne(entity);
        }

        /// <summary>
        /// See <see cref="IAsyncEditableRepository{TEntity, TKey}.DeleteAsync"/> for more information.
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        public void Delete(string id)
        {
            Collection.DeleteOne(x => x.Id == id);
        }

        /// <summary>
        /// See <see cref="IAsyncEditableRepository{TEntity, TKey}.UpdateAsync"/> for more information.
        /// </summary>
        /// <param name="entity">The mongo entity.</param>
        public void Update(TEntity entity)
        {
            Collection.ReplaceOne(x => x.Id == entity.Id, entity);
        }

        /// <summary>
        /// See <see cref="IAsyncReadableRepository{TEntity,TKey}.ReadAsync"/> for more information.
        /// </summary>
        /// <param name="predicate">The condition to find the entity.</param>
        /// <returns>The found entity.</returns>
        public TEntity Read(
            Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefault();
        }

        /// <summary>
        /// See <see cref="IAsyncReadableRepository{TEntity,TKey}.ReadManyAsync"/> for more information.
        /// </summary>
        /// <param name="predicate">The condition to find the entities.</param>
        /// <returns>The found entities.</returns>
        public IEnumerable<TEntity> ReadMany(
            Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).ToEnumerable();
        }
    }
}