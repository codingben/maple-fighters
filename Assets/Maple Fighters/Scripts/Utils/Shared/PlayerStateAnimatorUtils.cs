using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Utils.Shared
{
    public static class PlayerStateAnimatorUtils
    {
        public static void SetState(this Animator animator, PlayerState state)
        {
            // TODO: Do a better way to get name of the animations.
            const string WALK_NAME = "Walking";
            const string JUMP_NAME = "Jump";
            const string ROPE_NAME = "Rope";
            const string LADDER_NAME = "Ladder";

            animator?.SetBool(WALK_NAME, state == PlayerState.Moving);
            animator?.SetBool(JUMP_NAME, state == PlayerState.Falling);
            animator?.SetBool(ROPE_NAME, state == PlayerState.Rope);
            animator?.SetBool(LADDER_NAME, state == PlayerState.Ladder);
        }
    }
}