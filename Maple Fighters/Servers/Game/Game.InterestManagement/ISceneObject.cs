using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface ISceneObject : IEntity
    {
        int Id { get; }
        string Name { get; }

        IContainer<ISceneObject> Container { get; }
        IScene Scene { get; set; }

        void OnAwake();
    }
}