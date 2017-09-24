using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IScene
    {
        Vector2 RegionSize { get; }

        IRegion[,] GetAllRegions();

        IGameObject AddGameObject(IGameObject gameObject);
        void RemoveGameObject(int id);

        IGameObject GetGameObject(int gameObjectId);
    }
}