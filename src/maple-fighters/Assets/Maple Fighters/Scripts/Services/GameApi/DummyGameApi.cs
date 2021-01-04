using System;
using Game.Messages;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Services.GameApi
{
    public class DummyGameApi : MonoBehaviour, IGameApi
    {
        public static DummyGameApi GetInstance()
        {
            if (instance == null)
            {
                var gameApi = new GameObject("Dummy Game Api");
                instance = gameApi.AddComponent<DummyGameApi>();
            }

            return instance;
        }

        private static DummyGameApi instance;

        public Action<EnteredSceneMessage> SceneEntered { get; set; }

        public Action<SceneChangedMessage> SceneChanged { get; set; }

        public Action<GameObjectsAddedMessage> GameObjectsAdded { get; set; }

        public Action<GameObjectsRemovedMessage> GameObjectsRemoved { get; set; }

        public Action<PositionChangedMessage> PositionChanged { get; set; }

        public Action<AnimationStateChangedMessage> AnimationStateChanged { get; set; }

        public Action<AttackedMessage> Attacked { get; set; }

        public Action<BubbleNotificationMessage> BubbleMessageReceived { get; set; }

        public void SendMessage<TCode, TMessage>(TCode code, TMessage message)
            where TCode : IComparable, IFormattable, IConvertible
            where TMessage : class
        {
            var messageCode = (MessageCodes)Convert.ToByte(code);

            switch (messageCode)
            {
                case MessageCodes.ChangeScene:
                {
                    var changeSceneMessage = message as ChangeSceneMessage;
                    if (changeSceneMessage is ChangeSceneMessage)
                    {
                        HandleChangeSceneOperation(changeSceneMessage);
                    }

                    break;
                }
            }
        }

        private void HandleChangeSceneOperation(ChangeSceneMessage message)
        {
            var sceneChangedMessage = new SceneChangedMessage();
            var portalId = message.PortalId;
            if (portalId == 2)
            {
                sceneChangedMessage.Map = SceneIndexes.TheDarkForest;
            }
            else if (portalId == 3)
            {
                sceneChangedMessage.Map = SceneIndexes.Lobby;
            }

            SceneChanged?.Invoke(sceneChangedMessage);
        }
    }
}