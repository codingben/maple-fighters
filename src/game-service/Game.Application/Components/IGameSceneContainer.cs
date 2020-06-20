namespace Game.Application.Components
{
    public interface IGameSceneContainer
    {
        void AddScene(Map map, IGameScene scene);

        void RemoveScene(Map map);

        bool TryGetScene(Map map, out IGameScene scene);
    }
}