using Common.Repository;

namespace Common.MongoDB
{
    public interface IMongoRepository<TEntity> : IRepository<TEntity, string>
        where TEntity : IMongoEntity
    {
        // Left blank intentionally
    }
}