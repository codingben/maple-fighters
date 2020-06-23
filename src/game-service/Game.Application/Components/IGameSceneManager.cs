namespace Game.Application.Components
{
    public interface IGameSceneManager
    {
        bool TryGetGameScene(Map map, out IGameScene gameScene);
    }
}