using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface IPositionSetter
    {
        void Move(Vector2 position);
    }
}