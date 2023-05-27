using System;
using Game.Messages;
using Game.MessageTools;
using NativeWebSocket;

namespace Scripts.Services.GameApi
{
    public interface IGameApi
    {
        Action Connected { get; set; }

        Action<WebSocketCloseCode> Disconnected { get; set; }

        UnityEvent<EnteredSceneMessage> SceneEntered { get; set; }

        UnityEvent<SceneChangedMessage> SceneChanged { get; set; }

        UnityEvent<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        UnityEvent<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        UnityEvent<PositionChangedMessage> PositionChanged { get; set; }

        UnityEvent<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        UnityEvent<AttackedMessage> Attacked { get; set; }

        UnityEvent<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        UnityEvent<ChatMessage> ChatMessageReceived { get; set; }

        void SendMessage<T, M>(T code, M message)
            where T : IComparable, IFormattable, IConvertible
            where M : class;
    }
}