using System;
using Game.Messages;
using Game.MessageTools;
using NativeWebSocket;
using UnityEngine;

namespace Scripts.Services.GameApi
{
    public class DummyGameApi : MonoBehaviour, IGameApi
    {
        public static DummyGameApi GetInstance()
        {
            if (instance == null)
            {
                var gameApi = new GameObject("Dummy Game Api");
                instance = gameApi.AddComponent<DummyGameApi>();
            }

            return instance;
        }

        private static DummyGameApi instance;

        public Action Connected { get; set; }

        public Action<WebSocketCloseCode> Disconnected { get; set; }

        public UnityEvent<EnteredSceneMessage> SceneEntered { get; set; }

        public UnityEvent<SceneChangedMessage> SceneChanged { get; set; }

        public UnityEvent<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        public UnityEvent<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        public UnityEvent<PositionChangedMessage> PositionChanged { get; set; }

        public UnityEvent<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        public UnityEvent<AttackedMessage> Attacked { get; set; }

        public UnityEvent<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        public UnityEvent<ChatMessage> ChatMessageReceived { get; set; }

        private void Awake()
        {
            SceneEntered = new UnityEvent<EnteredSceneMessage>();
            SceneChanged = new UnityEvent<SceneChangedMessage>();
            GameObjectsAdded = new UnityEvent<GameObjectsAddedMessage>();
            GameObjectsRemoved = new UnityEvent<GameObjectsRemovedMessage>();
            PositionChanged = new UnityEvent<PositionChangedMessage>();
            AnimationStateChanged = new UnityEvent<AnimationStateChangedMessage>();
            Attacked = new UnityEvent<AttackedMessage>();
            BubbleMessageReceived = new UnityEvent<BubbleNotificationMessage>();
            ChatMessageReceived = new UnityEvent<ChatMessage>();
        }

        private void Start()
        {
            Connected?.Invoke();
        }

        private void OnDestroy()
        {
            ApiProvider.RemoveGameApiProvider();

            Disconnected?.Invoke(WebSocketCloseCode.Normal);
        }

        void IGameApi.SendMessage<T, M>(T code, M message)
        {
            var messageCode = (MessageCodes)Convert.ToByte(code);

            switch (messageCode)
            {
                case MessageCodes.ChangeScene:
                {
                    HandleChangeSceneOperation(message);
                    break;
                }
                case MessageCodes.ChatMessage:
                {
                    HandleChatMessageOperation(message);
                    break;
                }
            }
        }

        private void HandleChangeSceneOperation(object message)
        {
            var changeSceneMessage = (ChangeSceneMessage)message;
            var sceneChangedMessage = new SceneChangedMessage();

            var portalId = changeSceneMessage.PortalId;
            if (portalId == 3)
            {
                sceneChangedMessage.Map = 0; // Lobby
            }
            else if (portalId == 2)
            {
                sceneChangedMessage.Map = 1; // The Dark Forest
            }

            SceneChanged?.Invoke(sceneChangedMessage);
        }

        private void HandleChatMessageOperation(object message)
        {
            var chatMessage = (ChatMessage)message;
            var senderId = chatMessage.SenderId;
            var name = chatMessage.Name;
            var content = chatMessage.Content;

            // Format message
            chatMessage.ContentFormatted = $"<b>{name}: {content}</b>";

            // Send formatted message
            ChatMessageReceived?.Invoke(chatMessage);

            // Bubble notification (not formatted message)
            var bubbleNotificationMessage = new BubbleNotificationMessage()
            {
                NotifierId = senderId,
                Message = content,
                Time = 3 // Seconds
            };

            BubbleMessageReceived?.Invoke(bubbleNotificationMessage);
        }
    }
}