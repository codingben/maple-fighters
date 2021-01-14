using System;
using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using Game.Network;
using Game.Application.Handlers;
using Game.Messages;
using Game.Application.Objects.Components;
using Fleck;

namespace Game.Application
{
    public class GameService
    {
        private readonly IWebSocketConnection connection;
        private readonly IWebSocketSessionCollection webSocketSessionCollection;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly IGamePlayer gamePlayer;
        private readonly IMessageHandlerCollection messageHandlerCollection;

        public GameService(IWebSocketConnection connection, IComponents components)
        {
            // TODO: Unsubscribe
            this.connection = connection;
            this.connection.OnOpen += OnOpen;
            this.connection.OnClose += OnClose;
            this.connection.OnError += OnError;
            this.connection.OnBinary += OnBinary;

            webSocketSessionCollection = components.Get<IWebSocketSessionCollection>();
            gameSceneCollection = components.Get<IGameSceneCollection>();

            var idGenerator = components.Get<IIdGenerator>();
            var id = idGenerator.GenerateId();

            gamePlayer = new GamePlayer(id);
            messageHandlerCollection = new MessageHandlerCollection();
        }

        private void OnOpen()
        {
            AddPlayer();

            AddMessageSenderToPlayer();
            AddWebSocketSessionData();

            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
            AddHandlerForEnterScene();
            AddHandlerForChangeScene();
        }

        private void OnClose()
        {
            RemoveHandlerFromChangePosition();
            RemoveHandlerFromChangeAnimationState();
            RemoveHandlerFromEnterScene();
            RemoveHandlerFromChangeScene();

            RemoveWebSocketSessionData();

            RemovePlayer();
        }

        private void OnError(Exception exception)
        {
            Console.WriteLine($"OnError() -> {exception.Message}");
        }

        private void OnBinary(byte[] data)
        {
            var messageData =
                MessageUtils.DeserializeMessage<MessageData>(data);
            var code = messageData.Code;
            var rawData = messageData.RawData;

            if (messageHandlerCollection.TryGet(code, out var handler))
            {
                handler?.Invoke(rawData);
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

        private void RemoveHandlerFromChangePosition()
        {
            messageHandlerCollection.Unset(MessageCodes.ChangePosition);
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

        private void RemoveHandlerFromChangeAnimationState()
        {
            messageHandlerCollection.Unset(MessageCodes.ChangeAnimationState);
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

        private void RemoveHandlerFromEnterScene()
        {
            messageHandlerCollection.Unset(MessageCodes.EnterScene);
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

        private void RemoveHandlerFromChangeScene()
        {
            messageHandlerCollection.Unset(MessageCodes.ChangeScene);
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
                connection.Send(rawData);
            };

            messageSender.SendToMessageAction = (rawData, id) =>
            {
                if (webSocketSessionCollection.TryGet(id, out var webSocketSessionData))
                {
                    // TODO: Fix
                    // Sessions.SendTo(rawData, webSocketSessionData.Id);
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
                // TODO: Fix
                // var data = new WebSocketSessionData(ID);
                // webSocketSessionCollection.Add(player.Id, data);
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