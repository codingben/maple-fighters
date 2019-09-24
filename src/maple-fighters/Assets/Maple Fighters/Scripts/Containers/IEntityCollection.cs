using Game.Common;
using Scripts.Gameplay;

namespace Scripts.Containers
{
    public interface IEntityCollection
    {
        IEntity Add(SceneObjectParameters parameters);

        void Remove(int id);

        IEntity TryGet(int id);
    }
}