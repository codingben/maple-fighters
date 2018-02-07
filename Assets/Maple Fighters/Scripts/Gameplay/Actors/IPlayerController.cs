using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface IPlayerController
    {
        PlayerControllerConfig Config { get; }

        Rigidbody2D Rigidbody { get; }
        PlayerState PlayerState { set; get; }
        Directions Direction { set; }

        bool IsOnGround();
    }
}