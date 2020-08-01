using Common.ComponentModel;
using Common.Components;
using WebSocketSharp;
using WebSocketSharp.Server;
using Game.Application.Components;
using Game.Network;
using Game.Application.Handlers;
using Game.Messages;
using Game.Application.Objects.Components;

namespace Game.Application
{
    public class GameService : WebSocketBehavior
    {
        private readonly IWebSocketSessionCollection webSocketSessionCollection;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly IGamePlayer gamePlayer;
        private readonly IMessageHandlerCollection messageHandlerCollection;

        public GameService(IComponents components)
        {
            webSocketSessionCollection = components.Get<IWebSocketSessionCollection>();
            gameSceneCollection = components.Get<IGameSceneCollection>();

            var idGenerator = components.Get<IIdGenerator>();
            var id = idGenerator.GenerateId();

            gamePlayer = new GamePlayer(id);
            messageHandlerCollection = new MessageHandlerCollection();
        }

        protected override void OnOpen()
        {
            AddPlayer();
            AddMessageSenderToPlayer();
            AddWebSocketSessionData();
            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
            AddHandlerForEnterScene();
            AddHandlerForChangeScene();
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            RemoveWebSocketSessionData();
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

                if (messageHandlerCollection.TryGet(code, out var handler))
                {
                    handler?.Invoke(rawData);
                }
            }
        }

        private void AddHandlerForChangePosition()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageHandler = new ChangePositionMessageHandler(player);

                messageHandlerCollection.Set(MessageCodes.ChangePosition, messageHandler);
            }
        }

        private void AddHandlerForChangeAnimationState()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageHandler = new ChangeAnimationStateHandler(player);

                messageHandlerCollection.Set(MessageCodes.ChangeAnimationState, messageHandler);
            }
        }

        private void AddHandlerForEnterScene()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageHandler =
                    new EnterSceneMessageHandler(player, gameSceneCollection);

                messageHandlerCollection.Set(MessageCodes.EnterScene, messageHandler);
            }
        }

        private void AddHandlerForChangeScene()
        {
            var player = gamePlayer?.GetPlayer();
            if (player != null)
            {
                var messageHandler =
                    new ChangeSceneMessageHandler(player, gameSceneCollection);

                messageHandlerCollection.Set(MessageCodes.ChangeScene, messageHandler);
            }
        }

        private void AddPlayer()
        {
            gamePlayer?.Create();
        }

        private void AddMessageSenderToPlayer()
        {
            var player = gamePlayer?.GetPlayer();
            var messageSender = player?.Components.Get<IMessageSender>();

            messageSender.SendMessageAction = (rawData) =>
            {
                Send(rawData);
            };

            messageSender.SendToMessageAction = (rawData, id) =>
            {
                if (webSocketSessionCollection.TryGet(id, out var webSocketSessionData))
                {
                    Sessions.SendTo(rawData, webSocketSessionData.Id);
                }
            };
        }

        private void RemovePlayer()
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