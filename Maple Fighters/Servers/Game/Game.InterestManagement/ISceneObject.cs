using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface ISceneObject : IEntity<ISceneObject>
    {
        int Id { get; }
        string Name { get; }

        void OnAwake();
        void OnDestroy();
    }
}