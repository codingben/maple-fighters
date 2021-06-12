using System;
using System.Collections.Generic;
using Common.ComponentModel;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    /// <summary>
    /// This notifier is responsible for sending game objects to the client.
    /// 
    /// Here's how it works:
    /// ProximityChecker -> InterestManagementNotifier -> MessageSender -> GameClient
    /// </summary>
    public class InterestManagementNotifier : ComponentBase
    {
        private IProximityChecker proximityChecker;
        private IMessageSender messageSender;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            messageSender = Components.Get<IMessageSender>();

            var events = proximityChecker.GetNearbyGameObjectsEvents();
            events.SceneObjectAdded += OnSceneObjectAdded;
            events.SceneObjectRemoved += OnSceneObjectRemoved;
            events.SceneObjectsAdded += SceneObjectsAdded;
            events.SceneObjectsRemoved += SceneObjectsRemoved;
        }

        protected override void OnRemoved()
        {
            var events = proximityChecker.GetNearbyGameObjectsEvents();
            events.SceneObjectAdded -= OnSceneObjectAdded;
            events.SceneObjectRemoved -= OnSceneObjectRemoved;
            events.SceneObjectsAdded -= SceneObjectsAdded;
            events.SceneObjectsRemoved -= SceneObjectsRemoved;
        }

        private void OnSceneObjectAdded(IGameObject gameObject)
        {
            Console.WriteLine("OnSceneObjectAdded()");

            var messageCode = (byte)MessageCodes.GameObjectAdded;
            var message = new GameObjectsAddedMessage()
            {
                GameObjects = new GameObjectData[]
                {
                    GetGameObjectData(gameObject)
                }
            };

            messageSender.SendMessage(messageCode, message);
        }

        private void OnSceneObjectRemoved(IGameObject gameObject)
        {
            Console.WriteLine("OnSceneObjectRemoved()");

            var messageCode = (byte)MessageCodes.GameObjectRemoved;
            var message = new GameObjectsRemovedMessage()
            {
                GameObjectIds = new int[]
                {
                    gameObject.Id
                }
            };

            messageSender.SendMessage(messageCode, message);
        }

        private void SceneObjectsAdded(IEnumerable<IGameObject> collection)
        {
            Console.WriteLine("SceneObjectsAdded()");

            var gameObjectDatas = new List<GameObjectData>();

            foreach (var gameObject in collection)
            {
                gameObjectDatas.Add(GetGameObjectData(gameObject));
            }

            var messageCode = (byte)MessageCodes.GameObjectAdded;
            var message = new GameObjectsAddedMessage()
            {
                GameObjects = gameObjectDatas.ToArray()
            };

            messageSender.SendMessage(messageCode, message);
        }

        private void SceneObjectsRemoved(IEnumerable<IGameObject> collection)
        {
            Console.WriteLine("SceneObjectsRemoved()");

            var gameObjectIds = new List<int>();

            foreach (var gameObject in collection)
            {
                gameObjectIds.Add(gameObject.Id);
            }

            var messageCode = (byte)MessageCodes.GameObjectRemoved;
            var message = new GameObjectsRemovedMessage()
            {
                GameObjectIds = gameObjectIds.ToArray()
            };

            messageSender.SendMessage(messageCode, message);
        }

        private GameObjectData GetGameObjectData(IGameObject gameObject)
        {
            // TODO: A better way to get character
            var characterData = gameObject.Components.Get<ICharacterData>();
            var characterName = characterData?.GetName();
            var characterType = characterData?.GetCharacterType() ?? 0;

            return new GameObjectData()
            {
                Id = gameObject.Id,
                Name = gameObject.Name,
                X = gameObject.Transform.Position.X,
                Y = gameObject.Transform.Position.Y,
                Direction = gameObject.Transform.Size.X,
                CharacterName = characterName,
                CharacterClass = characterType,
                HasCharacter = characterData != null
            };
        }
    }
}