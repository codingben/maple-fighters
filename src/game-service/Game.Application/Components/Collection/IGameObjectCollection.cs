using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IGameObjectCollection
    {
        bool Add(IGameObject gameObject);

        bool Remove(int id);

        bool TryGet(int id, out IGameObject gameObject);
    }
}