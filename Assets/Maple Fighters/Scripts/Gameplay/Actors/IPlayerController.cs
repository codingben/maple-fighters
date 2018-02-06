using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface IPlayerController
    {
        Rigidbody2D Rigidbody { get; }
        Directions Direction { set; }
        PlayerState PlayerState { set; get; }

        bool IsOnGround();
    }
}