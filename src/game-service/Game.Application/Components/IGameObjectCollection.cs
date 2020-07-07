using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IGameObjectCollection
    {
        bool AddGameObject(IGameObject gameObject);

        bool RemoveGameObject(int id);

        bool TryGetGameObject(int id, out IGameObject gameObject);
    }
}