using Game.Common;
using UnityEngine;

namespace Scripts.Utils.Shared
{
    public static class PlayerStateAnimatorUtils
    {
        public static void ChangePlayerAnimationState(
            this Animator animator, PlayerState playerState)
        {
            // TODO: Hack
            if (playerState == PlayerState.Attacked)
            {
                playerState = PlayerState.Falling;
            }

            // TODO: Do a better way to get name of the animations
            const string WalkName = "Walking";
            const string JumpName = "Jump";
            const string RopeName = "Rope";
            const string LadderName = "Ladder";

            animator.SetBool(WalkName, playerState == PlayerState.Moving);
            animator.SetBool(JumpName, playerState == PlayerState.Jumping || playerState == PlayerState.Falling);
            animator.SetBool(RopeName, playerState == PlayerState.Rope);
            animator.SetBool(LadderName, playerState == PlayerState.Ladder);
        }
    }
}