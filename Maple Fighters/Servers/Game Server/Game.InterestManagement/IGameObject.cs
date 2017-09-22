using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public interface IGameObject : IEntity
    {
        int Id { get; }
        string Name { get; }

        IScene Scene { get; }

        IContainer<IGameObject> Container { get; }

        void SetScene(IScene scene);
        void RemoveScene();
    }
}