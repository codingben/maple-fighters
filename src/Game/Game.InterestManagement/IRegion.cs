namespace Game.InterestManagement
{
    public interface IRegion
    {
        void AddSceneObject(ISceneObject sceneObject);

        void RemoveSceneObject(ISceneObject sceneObject);
    }
}