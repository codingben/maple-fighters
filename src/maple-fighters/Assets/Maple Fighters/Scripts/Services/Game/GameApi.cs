using Game.Messages;
using NativeWebSocket;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class GameApi : MonoBehaviour, IGameApi
    {
        private WebSocket webSocket;

        private async void Start()
        {
            webSocket = new WebSocket("ws://localhost:50060");

            await webSocket.Connect();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit()
        {
            await webSocket.Close();
        }

        public async void ChangeAnimationState(ChangeAnimationStateMessage message)
        {
            throw new System.NotImplementedException();
        }

        public async void ChangePosition(ChangePositionMessage message)
        {
            throw new System.NotImplementedException();
        }

        public async void ChangeScene(ChangeSceneMessage message)
        {
            throw new System.NotImplementedException();
        }

        public async void EnterScene(EnterSceneMessage message)
        {
            throw new System.NotImplementedException();
        }

        private async void SendMessage(byte[] rawData)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.Send(rawData);
            }
        }
    }
}