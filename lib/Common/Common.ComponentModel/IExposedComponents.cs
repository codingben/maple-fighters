namespace Common.ComponentModel
{
    /// <summary>
    /// Represents only the exposed components in the component's container.
    /// NOTE: This provider should be accessed by other entities on other threads.
    /// </summary>
    public interface IExposedComponents
    {
        /// <summary>
        /// Adds and awakes a new exposed component to the component's container.
        /// NOTE: Please make sure this component is thread-safe if needed.
        /// </summary>
        /// <typeparam name="TComponent">An exposed component represented by an interface.</typeparam>
        /// <param name="component">The new component.</param>
        /// <returns>The new component after it was added to the container.</returns>
        TComponent Add<TComponent>(TComponent component)
            where TComponent : class;

        /// <summary>
        /// Searches for the exposed component on the component's container.
        /// </summary>
        /// <typeparam name="TComponent">An exposed component represented by an interface.</typeparam>
        /// <returns>The exposed component which will be found.</returns>
        TComponent Get<TComponent>()
            where TComponent : class;
    }
}