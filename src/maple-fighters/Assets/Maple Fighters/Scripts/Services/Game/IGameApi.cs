using System;
using Game.Messages;

namespace Scripts.Services.Game
{
    public interface IGameApi
    {
        Action<EnteredSceneMessage> SceneEntered { get; set; }

        Action<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        Action<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        Action<PositionChangedMessage> PositionChanged { get; set; }

        Action<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        Action<AttackedMessage> Attacked { get; set; }

        Action<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        void SendMessage<TCode, TMessage>(TCode code, TMessage message)
            where TCode : IComparable, IFormattable, IConvertible
            where TMessage : class;
    }
}