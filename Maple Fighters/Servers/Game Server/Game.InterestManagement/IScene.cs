namespace Game.InterestManagement
{
    public interface IScene
    {
        IRegion[,] GetAllRegions();

        IGameObject AddGameObject(IGameObject gameObject);
        void RemoveGameObjectFromScene(int id);

        IGameObject GetGameObject(int gameObjectId);
    }
}