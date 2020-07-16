namespace Common.ComponentModel
{
    public interface IComponents
    {
        /// <summary>
        /// Gets the component from the collection.
        /// </summary>
        /// <typeparam name="TComponent">The component represented by the interface.</typeparam>
        /// <returns>The component.</returns>
        TComponent Get<TComponent>()
            where TComponent : class;
    }
}