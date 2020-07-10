using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using WebSocketSharp;
using WebSocketSharp.Server;
using Game.Application.Objects.Components;
using Game.Application.Components;
using Game.Application.Network;
using Game.Application.Handlers;
using Game.Application.Messages;

namespace Game.Application
{
    public class GameService : WebSocketBehavior
    {
        private readonly IWebSocketSessionCollection webSocketSessionCollection;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly IGamePlayer gamePlayer;
        private readonly IDictionary<byte, IMessageHandler> handlers = new Dictionary<byte, IMessageHandler>();

        public GameService(IExposedComponents components)
        {
            webSocketSessionCollection = components.Get<IWebSocketSessionCollection>();
            gameSceneCollection = components.Get<IGameSceneCollection>();

            var idGenerator = components.Get<IIdGenerator>();
            var id = idGenerator.GenerateId();

            gamePlayer = new GamePlayer(id, gameSceneCollection);
        }

        protected override void OnOpen()
        {
            AddPlayer();
            AddSessionData();
            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
            AddHandlerForEnterScene();
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            RemoveSessionData();
            RemovePlayer();
        }

        protected override void OnError(ErrorEventArgs eventArgs)
        {
            // TODO: Logger.Log($"{eventArgs.Message}")
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
        }

        private void AddHandlerForChangePosition()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var transform = player.Transform;
                var handler = new ChangePositionMessageHandler(transform);

                handlers.Add((byte)MessageCodes.ChangePosition, handler);
            }
        }

        private void AddHandlerForChangeAnimationState()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var animationData = player.Components.Get<IAnimationData>();
                var handler = new ChangeAnimationStateHandler(animationData);

                handlers.Add((byte)MessageCodes.ChangeAnimationState, handler);
            }
        }

        private void AddHandlerForEnterScene()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var gameObjectGetter = player.Components.Get<IGameObjectGetter>();
                var characterData = player.Components.Get<ICharacterData>();
                var messageSender = player.Components.Get<IMessageSender>();
                var handler =
                    new EnterSceneMessageHandler(gameObjectGetter, characterData, messageSender);

                handlers.Add((byte)MessageCodes.EnterScene, handler);
            }
        }

        private void AddHandlerForChangeScene()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageSender = player.Components.Get<IMessageSender>();
                var presenceMapProvider = player.Components.Get<IPresenceMapProvider>();
                var proximityChecker = player.Components.Get<IProximityChecker>();
                var handler =
                    new ChangeSceneMessageHandler(messageSender, proximityChecker, presenceMapProvider, gameSceneCollection);

                handlers.Add((byte)MessageCodes.ChangeScene, handler);
            }
        }

        private void AddPlayer()
        {
            gamePlayer?.Create();
        }

        private void RemovePlayer()
        {
            gamePlayer?.Dispose();
        }

        private void AddSessionData()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var data = new WebSocketSessionData(ID);

                webSocketSessionCollection.AddSessionData(player.Id, data);
            }
        }

        private void RemoveSessionData()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                webSocketSessionCollection.RemoveSessionData(player.Id);
            }
        }
    }
}