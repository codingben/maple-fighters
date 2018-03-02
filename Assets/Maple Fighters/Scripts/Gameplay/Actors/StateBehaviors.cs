using System.Collections.Generic;
using CommonTools.Log;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class StateBehaviors : MonoBehaviour
    {
        private readonly Dictionary<PlayerState, IPlayerStateBehaviour> playerStateBehaviours = new Dictionary<PlayerState, IPlayerStateBehaviour>();

        protected void CreatePlayerStates()
        {
            foreach (var playerStateDetails in GetPlayerStatesDetails())
            {
                playerStateBehaviours.Add(playerStateDetails.PlayerState, playerStateDetails.PlayerStateBehaviour);
            }
        }

        private IEnumerable<PlayerStateDetails> GetPlayerStatesDetails()
        {
            yield return new PlayerStateDetails(PlayerState.Idle, new PlayerIdleState());
            yield return new PlayerStateDetails(PlayerState.Moving, new PlayerMovingState());
            yield return new PlayerStateDetails(PlayerState.Jumping, new PlayerJumpingState());
            yield return new PlayerStateDetails(PlayerState.Falling, new PlayerFallingState());
            yield return new PlayerStateDetails(PlayerState.Attacked, new PlayerAttackedState());

            var playerClimbingState = new PlayerClimbingState();
            yield return new PlayerStateDetails(PlayerState.Rope, playerClimbingState);
            yield return new PlayerStateDetails(PlayerState.Ladder, playerClimbingState);
        }

        protected IPlayerStateBehaviour GetStateBehaviour(PlayerState playerState)
        {
            IPlayerStateBehaviour playerStateBehaviour;

            if (playerStateBehaviours.TryGetValue(playerState, out playerStateBehaviour))
            {
                return playerStateBehaviour;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find behaviour of a {playerState} player state."));
            return null;
        }
    }
}