using Game.Application.Handlers;
using Game.Messages;
using Game.MessageTools;

namespace Game.Application.Components
{
    public class MessageHandlerManager : ComponentBase
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IMessageHandlerCollection handlerCollection;
        private readonly IGameSceneCollection gameSceneCollection;

        public MessageHandlerManager(IGameSceneCollection gameSceneCollection)
        {
            this.gameSceneCollection = gameSceneCollection;

            jsonSerializer = new NativeJsonSerializer();
            handlerCollection = new MessageHandlerCollection(jsonSerializer);
        }

        protected override void OnAwake()
        {
            var webSocketConnectionProvider =
                Components.Get<IWebSocketConnectionProvider>();
            var remotePlayerProvider =
                Components.Get<IRemotePlayerProvider>();
            var webSocketConnection =
                webSocketConnectionProvider.ProvideConnection();
            var remotePlayer =
                remotePlayerProvider.Provide();

            webSocketConnection.OnMessage += (json) =>
            {
                var messageData = jsonSerializer.Deserialize<MessageData>(json);
                var code = messageData.Code;
                var data = messageData.Data;

                if (handlerCollection.TryGet(code, out var handler))
                {
                    handler?.Invoke(data);
                }
            };

            handlerCollection.Set(MessageCodes.ChangePosition, new ChangePositionMessageHandler(remotePlayer));
            handlerCollection.Set(MessageCodes.ChangeAnimationState, new ChangeAnimationStateHandler(remotePlayer));
            handlerCollection.Set(MessageCodes.EnterScene, new EnterSceneMessageHandler(remotePlayer, gameSceneCollection));
            handlerCollection.Set(MessageCodes.ChangeScene, new ChangeSceneMessageHandler(remotePlayer, gameSceneCollection));
            handlerCollection.Set(MessageCodes.Attack, new AttackMessageHandler(remotePlayer));
        }

        protected override void OnRemoved()
        {
            handlerCollection?.Dispose();
        }
    }
}