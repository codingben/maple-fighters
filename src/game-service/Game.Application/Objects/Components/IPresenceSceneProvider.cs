using InterestManagement;

namespace Game.Application.Objects.Components
{
    public interface IPresenceSceneProvider
    {
        void SetScene(IScene<IGameObject> scene);

        IScene<IGameObject> GetScene();
    }
}