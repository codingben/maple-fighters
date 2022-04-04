namespace Common.Repository.Interfaces
{
    /// <summary>
    /// The type contained in the repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}