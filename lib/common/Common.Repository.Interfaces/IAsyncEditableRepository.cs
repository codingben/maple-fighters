using System.Threading.Tasks;

namespace Common.Repository.Interfaces
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
        /// <returns>The <see cref="Task"/>.</returns>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateAsync(TEntity entity);
    }
}