using System.Collections.Generic;
using Game.Application.Components;
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
            var messageCode = (byte)MessageCodes.GameObjectsAdded;
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
            var messageCode = (byte)MessageCodes.GameObjectsRemoved;
            var message = new GameObjectsRemovedMessage()
            {
                GameObjectIds = new int[]
                {
                    gameObject.Id
                }
            };

            messageSender.SendMessage(messageCode, message);
        }

        private void SceneObjectsAdded(IEnumerable<IGameObject> gameObjects)
        {
            var collection = new List<GameObjectData>();

            foreach (var gameObject in gameObjects)
            {
                collection.Add(GetGameObjectData(gameObject));
            }

            var messageCode = (byte)MessageCodes.GameObjectsAdded;
            var message = new GameObjectsAddedMessage()
            {
                GameObjects = collection.ToArray()
            };

            messageSender.SendMessage(messageCode, message);
        }

        private void SceneObjectsRemoved(IEnumerable<IGameObject> gameObjects)
        {
            var collection = new List<int>();

            foreach (var gameObject in gameObjects)
            {
                collection.Add(gameObject.Id);
            }

            var messageCode = (byte)MessageCodes.GameObjectsRemoved;
            var message = new GameObjectsRemovedMessage()
            {
                GameObjectIds = collection.ToArray()
            };

            messageSender.SendMessage(messageCode, message);
        }

        private GameObjectData GetGameObjectData(IGameObject gameObject)
        {
            var playerConfigDataProvider =
                gameObject.Components.Get<IPlayerConfigDataProvider>();
            var playerConfigData = playerConfigDataProvider?.Provide();

            return new GameObjectData()
            {
                Id = gameObject.Id,
                Name = gameObject.Name,
                X = gameObject.Transform.Position.X,
                Y = gameObject.Transform.Position.Y,
                Direction = 0, // TODO: Get direction
                CharacterName = playerConfigData?.CharacterName,
                CharacterClass = playerConfigData?.CharacterType ?? 0,
                HasCharacter = playerConfigData != null
            };
        }
    }
}