using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public interface ISceneObject : IEntity
    {
        int Id { get; }
        string Name { get; }

        IContainer<ISceneObject> Container { get; }
        IScene Scene { get; set; }
    }
}