using System;
using Game.Network;
using NativeWebSocket;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class GameApi : MonoBehaviour, IGameApi
    {
        private WebSocket webSocket;
        private IMessageHandlerCollection messageHandlerCollection;

        private async void Start()
        {
            webSocket = new WebSocket("ws://localhost:50060");
            messageHandlerCollection = new MessageHandlerCollection();

            await webSocket.Connect();

            SubscribeToMessageNotifier();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit()
        {
            UnsubscribeFromMessageNotifier();

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

        private void SubscribeToMessageNotifier()
        {
            webSocket.OnMessage += OnMessageReceived;
        }

        private void UnsubscribeFromMessageNotifier()
        {
            webSocket.OnMessage -= OnMessageReceived;
        }

        private void OnMessageReceived(byte[] data)
        {
            var messageData =
                MessageUtils.DeserializeMessage<MessageData>(data);
            var code = messageData.Code;
            var rawData = messageData.RawData;

            if (messageHandlerCollection.TryGet(code, out var handler))
            {
                handler?.Invoke(data);
            }
        }
    }
}