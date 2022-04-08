using System;
using Game.Application.Components;
using Game.Application.Handlers;
using Game.Messages;
using Game.MessageTools;
using Fleck;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application
{
    public class GameClient
    {
        private readonly int id;
        private readonly IWebSocketConnection connection;
        private readonly IWebSocketSessionCollection sessionCollection;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly IMessageHandlerCollection handlerCollection;
        private readonly IJsonSerializer jsonSerializer;
        private readonly IGameObject player;

        public GameClient(IWebSocketConnection connection, IComponents serverComponents)
        {
            this.connection = connection;

            connection.OnOpen += OnConnectionEstablished;
            connection.OnClose += OnConnectionClosed;
            connection.OnError += OnErrorOccurred;
            connection.OnMessage += OnMessageReceived;

            id = serverComponents.Get<IIdGenerator>().GenerateId();
            sessionCollection = serverComponents.Get<IWebSocketSessionCollection>();
            gameSceneCollection = serverComponents.Get<IGameSceneCollection>();

            jsonSerializer = new NativeJsonSerializer();
            handlerCollection = new MessageHandlerCollection(jsonSerializer);

            player = GetPlayerGameObject();

            Console.WriteLine($"A new client #{id} is connected.");
        }

        private void OnConnectionEstablished()
        {
            // General
            SubscribeToMessageSender();
            AddWebSocketSessionData();

            // Handlers
            handlerCollection.Set(MessageCodes.ChangePosition, new ChangePositionMessageHandler(player));
            handlerCollection.Set(MessageCodes.ChangeAnimationState, new ChangeAnimationStateHandler(player));
            handlerCollection.Set(MessageCodes.EnterScene, new EnterSceneMessageHandler(player, gameSceneCollection));
            handlerCollection.Set(MessageCodes.ChangeScene, new ChangeSceneMessageHandler(player, gameSceneCollection));
        }

        private void OnConnectionClosed()
        {
            // General
            UnsubscribeFromMessageSender();
            RemovePlayer();
            RemoveWebSocketSessionData();

            // Handlers
            handlerCollection.Unset(MessageCodes.ChangePosition);
            handlerCollection.Unset(MessageCodes.ChangeAnimationState);
            handlerCollection.Unset(MessageCodes.EnterScene);
            handlerCollection.Unset(MessageCodes.ChangeScene);
        }

        private void OnErrorOccurred(Exception exception)
        {
            Console.WriteLine($"OnErrorOccurred() -> {exception.Message}");
        }

        private void OnMessageReceived(string json)
        {
            var messageData = jsonSerializer.Deserialize<MessageData>(json);
            var code = messageData.Code;
            var data = messageData.Data;

            if (handlerCollection.TryGet(code, out var handler))
            {
                handler?.Invoke(data);
            }
        }

        private void SubscribeToMessageSender()
        {
            var messageSender = player.Components.Get<IMessageSender>();
            messageSender.SendMessageCallback += SendMessageCallback;
            messageSender.SendToMessageCallback += SendMessageCallback;
        }

        private void UnsubscribeFromMessageSender()
        {
            var messageSender = player.Components.Get<IMessageSender>();
            messageSender.SendMessageCallback -= SendMessageCallback;
            messageSender.SendToMessageCallback -= SendMessageCallback;
        }

        private void SendMessageCallback(string data)
        {
            connection.Send(data);
        }

        private void SendMessageCallback(string data, int id)
        {
            if (sessionCollection.TryGet(id, out var sessionData))
            {
                sessionData.Connection.Send(data);
            }
        }

        private void RemovePlayer()
        {
            var map = player.Components.Get<IPresenceMapProvider>().GetMap();
            map?.GameObjectCollection.Remove(id);

            player.Dispose();
        }

        private void AddWebSocketSessionData()
        {
            sessionCollection.Add(id, new WebSocketSessionData(id, connection));
        }

        private void RemoveWebSocketSessionData()
        {
            sessionCollection.Remove(id);
        }

        private IGameObject GetPlayerGameObject()
        {
            var components = new IComponent[]
            {
                new AnimationData(),
                new CharacterData(),
                new PresenceMapProvider(),
                new MessageSender(jsonSerializer),
                new PositionChangedMessageSender(),
                new AnimationStateChangedMessageSender(),
                new PlayerAttackedMessageSender(),
                new BubbleNotificationMessageSender(),
                new InterestManagementNotifier(),
                new PhysicsBodyPositionSetter()
            };

            return new GameObject(id, name: "RemotePlayer", components);
        }
    }
}