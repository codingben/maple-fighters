namespace Game.InterestManagement
{
    public interface IPresenceSceneProvider
    {
        IScene Scene { get; }

        void SetScene(IScene scene);
        void RemoveFromScene();
    }
}