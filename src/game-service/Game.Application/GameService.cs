using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using WebSocketSharp;
using WebSocketSharp.Server;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.Application.Components;
using Game.Application.Network;
using Game.Application.Handlers;
using Game.Application.Messages;

namespace Game.Application
{
    public class GameService : WebSocketBehavior
    {
        private readonly IIdGenerator idGenerator;
        private readonly ISessionDataCollection sessionDataCollection;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly IGameObject player;
        private readonly IDictionary<byte, IMessageHandler> handlers;

        public GameService(IExposedComponents components)
        {
            idGenerator = components.Get<IIdGenerator>();
            sessionDataCollection = components.Get<ISessionDataCollection>();
            gameSceneCollection = components.Get<IGameSceneCollection>();
            player = CreatePlayerGameObject();
            handlers = new Dictionary<byte, IMessageHandler>();
        }

        protected override void OnOpen()
        {
            sessionDataCollection.AddSessionData(player.Id, new SessionData(ID));

            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
            AddHandlerForEnterScene();
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            sessionDataCollection.RemoveSessionData(player.Id);

            RemoveHandlerFromChangePosition();
            RemoveHandlerFromChangeAnimationState();
            RemoveHandlerFromEnterScene();
        }

        protected override void OnError(ErrorEventArgs eventArgs)
        {
            // TODO: Log $"e.Message"
        }

        protected override void OnMessage(MessageEventArgs eventArgs)
        {
            if (eventArgs.IsBinary)
            {
                var messageData =
                    MessageUtils.DeserializeMessage<MessageData>(eventArgs.RawData);
                var code = messageData.Code;
                var rawData = messageData.RawData;

                if (handlers.TryGetValue(code, out var handler))
                {
                    handler?.Handle(rawData);
                }
            }
            else
            {
                // TODO: Log "Only binary data is allowed."
            }
        }

        private void AddHandlerForChangePosition()
        {
            var transform = player.Transform;
            var handler = new ChangePositionMessageHandler(transform);

            handlers.Add((byte)MessageCodes.ChangePosition, handler);
        }

        private void RemoveHandlerFromChangePosition()
        {
            handlers.Remove((byte)MessageCodes.ChangePosition);
        }

        private void AddHandlerForChangeAnimationState()
        {
            var animationData = player.Components.Get<IAnimationData>();
            var handler = new ChangeAnimationStateHandler(animationData);

            handlers.Add((byte)MessageCodes.ChangeAnimationState, handler);
        }

        private void RemoveHandlerFromChangeAnimationState()
        {
            handlers.Remove((byte)MessageCodes.ChangeAnimationState);
        }

        private void AddHandlerForEnterScene()
        {
            var gameObjectGetter = player.Components.Get<IGameObjectGetter>();
            var characterData = player.Components.Get<ICharacterData>();
            var messageSender = player.Components.Get<IMessageSender>();
            var handler =
                new EnterSceneMessageHandler(gameObjectGetter, characterData, messageSender);

            handlers.Add((byte)MessageCodes.EnterScene, handler);
        }

        private void RemoveHandlerFromEnterScene()
        {
            handlers.Remove((byte)MessageCodes.EnterScene);
        }

        private void AddHandlerForChangeScene()
        {
            var messageSender = player.Components.Get<IMessageSender>();
            var proximityChecker = player.Components.Get<IProximityChecker>();
            var presenceSceneProvider = player.Components.Get<IPresenceSceneProvider>();
            var handler = new ChangeSceneMessageHandler(
                messageSender,
                proximityChecker,
                gameSceneCollection,
                presenceSceneProvider);

            handlers.Add((byte)MessageCodes.ChangeScene, handler);
        }

        private void RemoveHandlerFromChangeScene()
        {
            handlers.Remove((byte)MessageCodes.ChangeScene);
        }

        public void SendMessageToMySession(byte[] rawData)
        {
            Send(rawData);
        }

        public void SendMessageToSession(byte[] rawData, int id)
        {
            if (sessionDataCollection.GetSessionData(id, out var sessionData))
            {
                Sessions.SendTo(rawData, sessionData.Id);
            }
        }

        private IGameObject CreatePlayerGameObject()
        {
            var id = idGenerator.GenerateId();
            var player = new GameObject(id, nameof(GameObjectType.Player));

            gameSceneCollection.TryGetScene(Map.Lobby, out var scene);

            var playerSpawnDataProvider = scene.Components.Get<IPlayerSpawnDataProvider>();
            var playerSpawnData = playerSpawnDataProvider.Provide();

            player.Transform.SetPosition(playerSpawnData.Position);
            player.Transform.SetSize(playerSpawnData.Size);

            // TODO: Dispose won't be called
            player.Components.Add(new GameObjectGetter(player));
            player.Components.Add(new AnimationData());
            player.Components.Add(new PresenceSceneProvider(scene));
            player.Components.Add(new ProximityChecker());
            player.Components.Add(new MessageSender(SendMessageToMySession, SendMessageToSession));
            player.Components.Add(new PositionChangedMessageSender());
            player.Components.Add(new AnimationStateChangedMessageSender());
            player.Components.Add(new CharacterData());

            return player;
        }
    }
}