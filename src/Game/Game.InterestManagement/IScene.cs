namespace Game.InterestManagement
{
    public interface IScene
    {
        IMatrixRegion MatrixRegion { get; }

        void AddSceneObject(ISceneObject sceneObject);

        void RemoveSceneObject(ISceneObject sceneObject);
    }
}