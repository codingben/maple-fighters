using System;
using Game.Network;
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

        async void IGameApi.SendMessage<TCode, TMessage>(TCode code, TMessage message)
        {
            var rawData =
                MessageUtils.WrapMessage(Convert.ToByte(code), message);

            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.Send(rawData);
            }
        }
    }
}