namespace Game.Application.Components
{
    public interface IGameSceneCollection
    {
        void AddScene(Map map, IGameScene scene);

        void RemoveScene(Map map);

        bool TryGetScene(Map map, out IGameScene scene);
    }
}