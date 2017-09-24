using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Rectangle Area { get; }

        private readonly Dictionary<int, InterestArea> gameObjects = new Dictionary<int, InterestArea>();

        public Region(Rectangle rectangle)
        {
            Area = rectangle;
        }

        public void AddSubscription(InterestArea interestArea)
        {
            if (gameObjects.ContainsKey(interestArea.Entity.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{interestArea.Entity.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            gameObjects.Add(interestArea.Entity.Id, interestArea);

            LogUtils.Log(MessageBuilder.Trace($"A new subscriber: {interestArea.Entity.Name}"));

            // Show all exists entities for a new game object.
            ShowGameObjectsForGameObject(interestArea);

            // Show a new game object for all exists entities.
            ShowGameObjectForGameObjects(interestArea);
        }

        public void RemoveSubscription(int gameObjectId)
        {
            if (!gameObjects.ContainsKey(gameObjectId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObjectId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            // Hide game objects for the one that left this region.
            HideGameObjectsForGameObject(gameObjectId);

            // Remove him from region's list.
            gameObjects.Remove(gameObjectId);

            // Hide the one who left from this region for other game objects.
            HideGameObjectForGameObjects(gameObjectId);
        }

        public void RemoveSubscriptionForOtherOnly(int gameObjectId)
        {
            if (!gameObjects.ContainsKey(gameObjectId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObjectId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            HideGameObjectsForGameObjectOnly(gameObjectId);

            // Remove him from region's list.
            gameObjects.Remove(gameObjectId);

            // Hide the one who left from this region for other game objects.
            HideGameObjectForOtherOnly(gameObjectId);
        }

        public bool HasSubscription(int gameObjectId)
        {
            return gameObjects.ContainsKey(gameObjectId);
        }

        public IEnumerable<InterestArea> GetAllSubscribers()
        {
            return gameObjects.Select(gameObject => gameObject.Value).ToList();
        }

        private void ShowGameObjectsForGameObject(InterestArea gameObject)
        {
            var gameObjectsTemp = gameObjects.Values.Where(gameObjectValue => gameObjectValue.Entity.Id != gameObject.Entity.Id).ToArray();
            gameObject?.GameObjectsAdded?.Invoke(gameObjectsTemp); 
        }

        private void HideGameObjectsForGameObject(int hideGameObjectId)
        {
            var gameObjectsTemp = gameObjects.Values.Where(gameObject => gameObject.Entity.Id != hideGameObjectId).ToArray();

            var interestArea = gameObjects[hideGameObjectId];

            var removeGameObjects = new List<int>();

            foreach (var gameObject in gameObjectsTemp)
            {
                if (interestArea.GetPublishers().Any(publisher => !publisher.HasSubscription(gameObject.Entity.Id)))
                {
                    removeGameObjects.Add(gameObject.Entity.Id);
                }
            }

            if (removeGameObjects.Count > 0)
            {
                interestArea.GameObjectsRemoved?.Invoke(removeGameObjects.ToArray());
            }
        }

        private void ShowGameObjectForGameObjects(InterestArea newGameObject)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Value.Entity.Id == newGameObject.Entity.Id)
                {
                    continue;
                }

                gameObject.Value.GameObjectAdded?.Invoke(newGameObject);
            }
        }

        private void HideGameObjectForGameObjects(int hideGameObjectId)
        {
            foreach (var gameObject in gameObjects.Values)
            {
                if (!gameObject.GetPublishers().Any(publisher => publisher.HasSubscription(hideGameObjectId)))
                {
                    gameObject.GameObjectRemoved?.Invoke(hideGameObjectId);
                }
            }
        }

        private void HideGameObjectForOtherOnly(int hideGameObjectId)
        {
            foreach (var gameObject in gameObjects.Values)
            {
                gameObject.GameObjectRemoved?.Invoke(hideGameObjectId);
            }
        }

        private void HideGameObjectsForGameObjectOnly(int hideGameObjectId)
        {
            var gameObjectsTemp = gameObjects.Keys.Where(gameObjectId => gameObjectId != hideGameObjectId).ToArray();

            if (gameObjects[hideGameObjectId] == null)
            {
                return;
            }

            gameObjects[hideGameObjectId]?.GameObjectsRemoved?.Invoke(gameObjectsTemp);
        }
    }
}