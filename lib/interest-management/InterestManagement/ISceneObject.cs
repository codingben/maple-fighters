namespace InterestManagement
{
    public interface ISceneObject
    {
        /// <summary>
        /// Gets scene object id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the transform of the scene object.
        /// </summary>
        ITransform Transform { get; }
    }
}