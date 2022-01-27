namespace Game.Application.Components
{
    public interface IGameSceneCollection
    {
        bool Add(Map map, IGameScene gameScene);

        void Remove(Map map);

        bool TryGet(Map map, out IGameScene gameScene);
    }
}