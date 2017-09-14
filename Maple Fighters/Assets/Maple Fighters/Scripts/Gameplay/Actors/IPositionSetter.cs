using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface IPositionSetter
    {
        void SetPosition(Vector2 newPosition, Directions direction);
    }
}