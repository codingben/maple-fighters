using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IGameObjectCollection
    {
        bool TryGetGameObject(int id, out IGameObject gameObject);
    }
}