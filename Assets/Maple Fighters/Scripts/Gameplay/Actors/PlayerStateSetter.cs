using Scripts.Utils.Shared;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerStateSetter : MonoBehaviour
    {
        public Animator Animator { private get; set; }

        public void SetState(PlayerState playerState)
        {
            Animator.SetState(playerState);
        }
    }
}