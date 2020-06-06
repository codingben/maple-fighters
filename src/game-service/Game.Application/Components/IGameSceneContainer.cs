using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public interface IGameSceneContainer
    {
        bool TryGetScene(Map map, out IScene<IGameObject> scene);
    }
}