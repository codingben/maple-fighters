using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repository
{
    /// <summary>
    /// Represents the readable repository definition.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TKey">The entity's Id.</typeparam>
    public interface IAsyncReadableRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Finds an entity which satisfied to the prediction.
        /// </summary>
        /// <param name="predicate">The prediction.</param>
        /// <returns>Returns the found entity.</returns>
        Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds the entities which satisfied to the prediction.
        /// </summary>
        /// <param name="predicate">The prediction.</param>
        /// <returns>Returns the found entities.</returns>
        Task<IEnumerable<TEntity>> ReadManyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}