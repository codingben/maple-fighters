namespace Common.ComponentModel
{
    public enum LifeTime
    {
        /// <summary>
        /// Creates a one-time component in the process.
        /// NOTE: This component should be a thread safe if exposed.
        /// </summary>
        Singleton,

        /// <summary>
        /// Returns a new component per-thread basis.
        /// </summary>
        PerThread,

        /// <summary>
        /// Returns a new component per call.
        /// </summary>
        PerCall
    }
}