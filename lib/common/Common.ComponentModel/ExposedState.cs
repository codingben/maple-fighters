namespace Common.ComponentModel
{
    public enum ExposedState
    {
        /// <summary>
        /// Not allows to other entities on other threads to access this component.
        /// </summary>
        Unexposable,

        /// <summary>
        /// Allows other entities on other threads to access this component.
        /// </summary>
        Exposable
    }
}