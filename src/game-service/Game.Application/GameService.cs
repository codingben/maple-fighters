using System;
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
using Common.MathematicsHelper;

namespace Game.Application
{
    // TODO: The purpose of this class is to process only messages
    public class GameService : WebSocketBehavior
    {
        private readonly IIdGenerator idGenerator;
        private readonly ISessionDataCollection sessionDataCollection;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly IDictionary<byte, IMessageHandler> handlers = new Dictionary<byte, IMessageHandler>();

        private IGameObject player;

        public GameService(IExposedComponents components)
        {
            idGenerator = components.Get<IIdGenerator>();
            sessionDataCollection = components.Get<ISessionDataCollection>();
            gameSceneCollection = components.Get<IGameSceneCollection>();
        }

        protected override void OnOpen()
        {
            CreatePlayer();

            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
            AddHandlerForEnterScene();

            sessionDataCollection.AddSessionData(player.Id, new SessionData(ID));
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            RemovePlayer();

            sessionDataCollection.RemoveSessionData(player.Id);
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

        private void AddHandlerForChangeAnimationState()
        {
            var animationData = player.Components.Get<IAnimationData>();
            var handler = new ChangeAnimationStateHandler(animationData);

            handlers.Add((byte)MessageCodes.ChangeAnimationState, handler);
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

        private void AddHandlerForChangeScene()
        {
            var messageSender = player.Components.Get<IMessageSender>();
            var presenceMapProvider = player.Components.Get<IPresenceMapProvider>();
            var proximityChecker = player.Components.Get<IProximityChecker>();
            var handler =
                new ChangeSceneMessageHandler(messageSender, proximityChecker, presenceMapProvider, gameSceneCollection);

            handlers.Add((byte)MessageCodes.ChangeScene, handler);
        }

        // TODO: Remove
        private void CreatePlayer()
        {
            var id = idGenerator.GenerateId();

            Action<byte[], int> sendTo = (rawData, id) =>
            {
                if (sessionDataCollection.GetSessionData(id, out var sessionData))
                {
                    Sessions.SendTo(rawData, sessionData.Id);
                }
            };

            Action<byte[]> send = (rawData) =>
            {
                Send(rawData);
            };

            var playerPosition = new Vector2(18, -1.86f);

            player = new PlayerGameObject(id, playerPosition);
            player.Components.Add(new MessageSender(send, sendTo));
            player.Components.Add(new AnimationData());
            player.Components.Add(new PositionChangedMessageSender());
            player.Components.Add(new AnimationStateChangedMessageSender());
            player.Components.Add(new CharacterData());

            var map = Map.Lobby;

            if (gameSceneCollection.TryGet(map, out var gameScene))
            {
                var proximityChecker = player.Components.Get<IProximityChecker>();
                var region = gameScene.MatrixRegion;

                proximityChecker.SetMatrixRegion(region);
            }

            // The Dark Forest: new Vector2(-12.8f, -2.95f)

            AddPlayerToGameScene(map);
        }

        private void AddPlayerToGameScene(Map map)
        {
            if (gameSceneCollection.TryGet(map, out var gameScene))
            {
                gameScene.GameObjectCollection.Add(player);
            }
            else
            {
                // TODO: Throw the error "Could not enter the world of the game"
            }
        }

        // TODO: Remove
        private void RemovePlayer()
        {
            var presenceMapProvider = player.Components.Get<IPresenceMapProvider>();
            var map = presenceMapProvider.GetMap();

            RemovePlayerFromGameScene((Map)map);

            player.Dispose();
        }

        private void RemovePlayerFromGameScene(Map map)
        {
            if (gameSceneCollection.TryGet(map, out var gameScene))
            {
                gameScene.GameObjectCollection.Remove(player.Id);
            }
        }
    }
}