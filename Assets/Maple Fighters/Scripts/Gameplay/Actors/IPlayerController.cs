using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface IPlayerController
    {
        Rigidbody2D Rigidbody { get; }
        PlayerState PlayerState { get; set; }

        bool IsOnGround();
    }
}