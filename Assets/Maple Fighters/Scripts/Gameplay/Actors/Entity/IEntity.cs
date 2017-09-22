using GameObject = UnityEngine.GameObject;

namespace Scripts.Gameplay.Actors.Entity
{
    public interface IEntity
    {
        int Id { get; }

        GameObject GameObject { get; }
    }
}