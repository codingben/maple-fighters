using Game.Common;
using UnityEngine;

namespace Scripts.Utils.Shared
{
    public static class PlayerStateAnimatorUtils
    {
        public static void ChangePlayerAnimationState(
            this Animator animator, 
            PlayerState playerState)
        {
            // TODO: Hack
            if (playerState == PlayerState.Attacked)
            {
                playerState = PlayerState.Falling;
            }

            animator.SetBool("WalkName", playerState == PlayerState.Moving);
            animator.SetBool("JumpName", playerState == PlayerState.Jumping || playerState == PlayerState.Falling);
            animator.SetBool("RopeName", playerState == PlayerState.Rope);
            animator.SetBool("LadderName", playerState == PlayerState.Ladder);
        }
    }
}