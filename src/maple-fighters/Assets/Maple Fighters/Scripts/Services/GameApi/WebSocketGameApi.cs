using System;
using System.Text;
using Game.Messages;
using Game.MessageTools;
using NativeWebSocket;
using UnityEngine;

namespace Scripts.Services.GameApi
{
    public class WebSocketGameApi : MonoBehaviour, IGameApi
    {
        public static WebSocketGameApi GetInstance()
        {
            if (instance == null)
            {
                var gameApi = new GameObject("WebSocket Game Api");
                instance = gameApi.AddComponent<WebSocketGameApi>();
            }

            return instance;
        }

        private static WebSocketGameApi instance;

        public Action<EnteredSceneMessage> SceneEntered { get; set; }

        public Action<SceneChangedMessage> SceneChanged { get; set; }

        public Action<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        public Action<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        public Action<PositionChangedMessage> PositionChanged { get; set; }

        public Action<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        public Action<AttackedMessage> Attacked { get; set; }

        public Action<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        private IJsonSerializer jsonSerializer;

        private MessageHandlerCollection collection;
        private WebSocket webSocket;

        private void Awake()
        {
            jsonSerializer = new UnityJsonSerializer();
            collection = new MessageHandlerCollection(jsonSerializer);
        }

        private async void Start()
        {
            var url = UserData.GameServerUrl;

            webSocket = new WebSocket(url);
            webSocket.OnOpen += OnOpen;
            webSocket.OnClose += OnClose;
            webSocket.OnMessage += OnMessage;

            if (webSocket != null)
            {
                await webSocket.Connect();
            }
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket?.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit()
        {
            if (webSocket != null)
            {
                await webSocket.Close();
            }
        }

        async void IGameApi.SendMessage<TCode, TMessage>(TCode code, TMessage message)
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                var data = jsonSerializer.Serialize(new MessageData()
                {
                    Code = Convert.ToByte(code),
                    Data = jsonSerializer.Serialize(message)
                });

                if (webSocket != null)
                {
                    await webSocket.SendText(data);
                }
            }
        }

        private void OnOpen()
        {
            collection.Set(MessageCodes.EnteredScene, SceneEntered.ToMessageHandler());
            collection.Set(MessageCodes.ChangeScene, SceneChanged.ToMessageHandler());
            collection.Set(MessageCodes.GameObjectAdded, GameObjectsAdded.ToMessageHandler());
            collection.Set(MessageCodes.GameObjectRemoved, GameObjectsRemoved.ToMessageHandler());
            collection.Set(MessageCodes.PositionChanged, PositionChanged.ToMessageHandler());
            collection.Set(MessageCodes.AnimationStateChanged, AnimationStateChanged.ToMessageHandler());
            collection.Set(MessageCodes.Attacked, Attacked.ToMessageHandler());
            collection.Set(MessageCodes.BubbleNotification, BubbleMessageReceived.ToMessageHandler());
        }

        private void OnClose(WebSocketCloseCode closeCode)
        {
            collection.Unset(MessageCodes.EnteredScene);
            collection.Unset(MessageCodes.ChangeScene);
            collection.Unset(MessageCodes.GameObjectAdded);
            collection.Unset(MessageCodes.GameObjectRemoved);
            collection.Unset(MessageCodes.PositionChanged);
            collection.Unset(MessageCodes.AnimationStateChanged);
            collection.Unset(MessageCodes.Attacked);
            collection.Unset(MessageCodes.BubbleNotification);
        }

        private void OnMessage(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            var messageData = jsonSerializer.Deserialize<MessageData>(message);
            var code = messageData.Code;
            var data = messageData.Data;

            if (collection.TryGet(code, out var handler))
            {
                handler?.Invoke(data);
            }
        }
    }
}