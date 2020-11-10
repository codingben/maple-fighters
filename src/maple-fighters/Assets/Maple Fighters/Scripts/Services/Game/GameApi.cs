using System;
using Game.Messages;
using Game.Network;
using NativeWebSocket;
using ScriptableObjects.Configurations;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class GameApi : MonoBehaviour, IGameApi
    {
        public Action<EnteredSceneMessage> SceneEntered { get; set; }

        public Action<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        public Action<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        public Action<PositionChangedMessage> PositionChanged { get; set; }

        public Action<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        public Action<AttackedMessage> Attacked { get; set; }

        public Action<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        private WebSocket webSocket;
        private IMessageHandlerCollection messageHandlerCollection;

        private async void Start()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            var gameServerData = networkConfiguration.GetServerData(ServerType.Game);
            var gameServerUrl = gameServerData.Url;

            webSocket = new WebSocket(gameServerUrl);
            messageHandlerCollection = new MessageHandlerCollection();
            messageHandlerCollection.Set(MessageCodes.EnteredScene, SceneEntered.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.GameObjectAdded, GameObjectsAdded.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.GameObjectRemoved, GameObjectsRemoved.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.PositionChanged, PositionChanged.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.AnimationStateChanged, AnimationStateChanged.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.Attacked, Attacked.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.BubbleNotification, BubbleMessageReceived.ToMessageHandler());

            await webSocket?.Connect();

            SubscribeToMessageNotifier();
            SetMessageHandlers();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket?.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit()
        {
            UnsubscribeFromMessageNotifier();
            UnsetMessageHandlers();

            await webSocket?.Close();
        }

        async void IGameApi.SendMessage<TCode, TMessage>(TCode code, TMessage message)
        {
            var rawData =
                MessageUtils.WrapMessage(Convert.ToByte(code), message);

            if (webSocket?.State == WebSocketState.Open)
            {
                await webSocket?.Send(rawData);
            }
        }

        private void SetMessageHandlers()
        {
            messageHandlerCollection.Set(MessageCodes.EnteredScene, SceneEntered.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.GameObjectAdded, GameObjectsAdded.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.GameObjectRemoved, GameObjectsRemoved.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.PositionChanged, PositionChanged.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.AnimationStateChanged, AnimationStateChanged.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.Attacked, Attacked.ToMessageHandler());
            messageHandlerCollection.Set(MessageCodes.BubbleNotification, BubbleMessageReceived.ToMessageHandler());
        }

        private void UnsetMessageHandlers()
        {
            messageHandlerCollection.Unset(MessageCodes.EnteredScene);
            messageHandlerCollection.Unset(MessageCodes.GameObjectAdded);
            messageHandlerCollection.Unset(MessageCodes.GameObjectRemoved);
            messageHandlerCollection.Unset(MessageCodes.PositionChanged);
            messageHandlerCollection.Unset(MessageCodes.AnimationStateChanged);
            messageHandlerCollection.Unset(MessageCodes.Attacked);
            messageHandlerCollection.Unset(MessageCodes.BubbleNotification);
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