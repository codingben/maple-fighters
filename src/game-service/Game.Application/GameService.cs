using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using WebSocketSharp;
using WebSocketSharp.Server;
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

            gamePlayer = new GamePlayer(id);
        }

        protected override void OnOpen()
        {
            AddPlayerGameObject();
            AddWebSocketSessionData();
            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
            AddHandlerForEnterScene();
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            RemoveWebSocketSessionData();
            RemovePlayerGameObject();
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
                var messageCode = (byte)MessageCodes.ChangePosition;
                var messageHandler = new ChangePositionMessageHandler(player);

                handlers.Add(messageCode, messageHandler);
            }
        }

        private void AddHandlerForChangeAnimationState()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageCode = (byte)MessageCodes.ChangeAnimationState;
                var messageHandler = new ChangeAnimationStateHandler(player);

                handlers.Add(messageCode, messageHandler);
            }
        }

        private void AddHandlerForEnterScene()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageCode = (byte)MessageCodes.EnterScene;
                var messageHandler =
                    new EnterSceneMessageHandler(player, gameSceneCollection);

                handlers.Add(messageCode, messageHandler);
            }
        }

        private void AddHandlerForChangeScene()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageCode = (byte)MessageCodes.ChangeScene;
                var messageHandler =
                    new ChangeSceneMessageHandler(player, gameSceneCollection);

                handlers.Add(messageCode, messageHandler);
            }
        }

        private void AddPlayerGameObject()
        {
            gamePlayer?.Create();
        }

        private void RemovePlayerGameObject()
        {
            gamePlayer?.Dispose();
        }

        private void AddWebSocketSessionData()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var data = new WebSocketSessionData(ID);

                webSocketSessionCollection.Add(player.Id, data);
            }
        }

        private void RemoveWebSocketSessionData()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                webSocketSessionCollection.Remove(player.Id);
            }
        }
    }
}