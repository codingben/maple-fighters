namespace Game.InterestManagement
{
    public interface IScene
    {
        void AddSceneObject(ISceneObject sceneObject);

        void RemoveSceneObject(ISceneObject sceneObject);
    }
}