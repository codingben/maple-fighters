using Game.Messages;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class GameApi : MonoBehaviour, IGameApi
    {
        public void ChangeAnimationState(ChangeAnimationStateMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void ChangePosition(ChangePositionMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeScene(ChangeSceneMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void EnterScene(EnterSceneMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}