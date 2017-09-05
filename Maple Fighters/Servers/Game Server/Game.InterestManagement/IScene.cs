namespace Game.InterestManagement
{
    public interface IScene
    {
        IRegion[,] GetAllRegions();

        IGameObject AddGameObject(IGameObject gameObject);
        void RemoveGameObject(IGameObject gameObject);

        IGameObject GetGameObject(int gameObjectId);
    }
}