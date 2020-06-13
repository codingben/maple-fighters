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
        private readonly IExposedComponents components;
        private readonly ISessionDataContainer sessionDataContainer;
        private readonly IDictionary<byte, IMessageHandler> handlers;
        private readonly IGameObject player;

        public GameService(IExposedComponents components)
        {
            this.components = components;
            this.sessionDataContainer = components.Get<ISessionDataContainer>();

            handlers = new Dictionary<byte, IMessageHandler>();
            player = CreatePlayerGameObject();
        }

        protected override void OnOpen()
        {
            sessionDataContainer.AddSessionData(player.Id, new SessionData(ID));

            AddHandlerForChangePosition();
            AddHandlerForChangeAnimationState();
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            sessionDataContainer.RemoveSessionData(player.Id);

            RemoveHandlerForChangePosition();
            RemoveHandlerForChangeAnimationState();
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
            var proximityChecker = player.Components.Get<IProximityChecker>();
            var handler =
                new ChangePositionMessageHandler(transform, proximityChecker, SendMessage);

            handlers.Add((byte)MessageCodes.ChangePosition, handler);
        }

        private void RemoveHandlerForChangePosition()
        {
            handlers.Remove((byte)MessageCodes.ChangePosition);
        }

        private void AddHandlerForChangeAnimationState()
        {
            var animationData = player.Components.Get<IAnimationData>();
            var proximityChecker = player.Components.Get<IProximityChecker>();
            var handler =
                new ChangeAnimationStateHandler(animationData, proximityChecker, SendMessage);

            handlers.Add((byte)MessageCodes.ChangeAnimationState, handler);
        }

        private void RemoveHandlerForChangeAnimationState()
        {
            handlers.Remove((byte)MessageCodes.ChangeAnimationState);
        }

        public void SendMessage(byte[] data, int id)
        {
            if (sessionDataContainer.GetSessionData(id, out var sessionData))
            {
                Sessions.SendTo(data, sessionData.Id);
            }
        }

        private IGameObject CreatePlayerGameObject()
        {
            var idGenerator = components.Get<IIdGenerator>();
            var id = idGenerator.GenerateId();
            var player = new GameObject(id, nameof(GameObjectType.Player));
            var gameSceneContainer = components.Get<IGameSceneContainer>();
            gameSceneContainer.TryGetScene(Map.Lobby, out var scene);

            player.Transform.SetPosition(scene.PlayerSpawnData.Position);
            player.Transform.SetSize(scene.PlayerSpawnData.Size);

            // TODO: Dispose won't be called
            player.Components.Add(new GameObjectGetter(player));
            player.Components.Add(new AnimationData());
            player.Components.Add(new PresenceSceneProvider(scene));
            player.Components.Add(new ProximityChecker());

            return player;
        }
    }
}