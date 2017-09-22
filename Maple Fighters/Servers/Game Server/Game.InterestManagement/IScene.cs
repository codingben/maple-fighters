namespace Game.InterestManagement
{
    public interface IScene
    {
        IRegion[,] GetAllRegions();

        IGameObject AddGameObject(IGameObject gameObject);
        void RemoveGameObject(int id);

        IGameObject GetGameObject(int gameObjectId);
    }
}