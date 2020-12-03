namespace InterestManagement.Components.Interfaces
{
    public interface IPresenceSceneProvider
    {
        void SetScene(IScene scene);
        IScene GetScene();
    }
}