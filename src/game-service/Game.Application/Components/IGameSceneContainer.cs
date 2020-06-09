namespace Game.Application.Components
{
    public interface IGameSceneContainer
    {
        bool TryGetScene(Map map, out IGameScene scene);
    }
}