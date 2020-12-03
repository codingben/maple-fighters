using ComponentModel.Common;

namespace InterestManagement.Components.Interfaces
{
    public interface ISceneObject : IEntity<ISceneObject>
    {
        int Id { get; }
        string Name { get; }

        void Awake();
    }
}