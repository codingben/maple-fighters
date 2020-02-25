namespace Common.Repository
{
    /// <summary>
    /// Represents the editable and readable repository definitions.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TKey">The entity's Id.</typeparam>
    public interface IRepository<TEntity, TKey> : IEditableRepository<TEntity, TKey>,
                                                  IReadableRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        // Left blank intentionally
    }
}