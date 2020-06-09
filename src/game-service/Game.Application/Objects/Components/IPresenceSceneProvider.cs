using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public interface IPresenceSceneProvider
    {
        void SetScene(IGameScene scene);

        IGameScene GetScene();
    }
}