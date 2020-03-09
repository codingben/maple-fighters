namespace Common.Repository
{
    /// <summary>
    /// Represents the editable and readable repository definitions.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TKey">The entity's Id.</typeparam>
    public interface IAsyncRepository<TEntity, TKey> : IAsyncEditableRepository<TEntity, TKey>,
                                                       IAsyncReadableRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        // Left blank intentionally
    }
}