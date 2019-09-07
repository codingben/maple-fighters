using Game.Common;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterOrientation
    {
        Directions GetDirection();

        void SetDirection(Directions direction);
    }
}