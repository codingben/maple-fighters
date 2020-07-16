namespace Common.ComponentModel
{
    public interface IComponents
    {
        /// <summary>
        /// Get a component from the collection only through the interface.
        /// </summary>
        /// <typeparam name="TComponent">The component represented by the interface.</typeparam>
        /// <returns>The component.</returns>
        TComponent Get<TComponent>()
            where TComponent : class;
    }
}