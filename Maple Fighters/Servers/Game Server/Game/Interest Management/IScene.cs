using Game.Entities;

namespace Game.InterestManagement
{
    internal interface IScene
    {
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        IEntity GetEntity(int entityId);

        IRegion[,] GetAllRegions();
    }
}