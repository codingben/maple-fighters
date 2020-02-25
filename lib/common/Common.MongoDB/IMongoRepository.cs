using Common.Repository;

namespace Common.MongoDB
{
    /// <summary>
    /// Represents the mongo repository.
    /// </summary>
    /// <typeparam name="TEntity">The mongo entity.</typeparam>
    public interface IMongoRepository<TEntity> : IRepository<TEntity, string>
        where TEntity : IMongoEntity
    {
        // Left blank intentionally
    }
}