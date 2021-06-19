using System.Collections.Generic;
using Common.ComponentModel;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    /// <summary>
    /// The interest management notifier is responsible for sending
    /// game objects to the client.
    /// 
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
            var characterName = characterData?.Name;
            var characterType = characterData?.CharacterType ?? 0;

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