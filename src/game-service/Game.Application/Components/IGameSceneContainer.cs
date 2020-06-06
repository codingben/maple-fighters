using InterestManagement;

namespace Game.Application.Components
{
    public interface IGameSceneContainer
    {
        bool TryGetScene(Map map, out IScene<ISceneObject> scene);
    }
}