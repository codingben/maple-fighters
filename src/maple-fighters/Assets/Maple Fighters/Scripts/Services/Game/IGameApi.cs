using System;
using Game.Messages;

namespace Scripts.Services.Game
{
    public interface IGameApi
    {
        Action<EnteredSceneMessage> SceneEntered { get; }

        Action<GameObjectsAddedMessage> SceneObjectsAdded { get; }

        Action<GameObjectsRemovedMessage> SceneObjectsRemoved { get; }

        Action<PositionChangedMessage> PositionChanged { get; }

        Action<AnimationStateChangedMessage> AnimationStateChanged { get; }

        Action<AttackedMessage> Attacked { get; }

        Action<BubbleNotificationMessage> BubbleMessageReceived { get; }

        void SendMessage<TCode, TMessage>(TCode code, TMessage message)
            where TCode : IComparable, IFormattable, IConvertible
            where TMessage : class;
    }
}