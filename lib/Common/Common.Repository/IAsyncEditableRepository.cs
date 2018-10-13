using System.Threading.Tasks;

namespace Common.Repository
{
    /// <summary>
    /// Represents the editable repository definition.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TKey">The entity's Id.</typeparam>
    public interface IAsyncEditableRepository<in TEntity, in TKey>
        where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Creates the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task UpdateAsync(TEntity entity);
    }
}