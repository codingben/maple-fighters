using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Rectangle Area { get; }

        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        public Region(Rectangle rectangle)
        {
            Area = rectangle;
        }

        public void AddSubscription(IGameObject gameObject)
        {
            if (gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            gameObjects.Add(gameObject.Id, gameObject);

            // Show all exists entities for a new game object.
            ShowGameObjectsForGameObject(gameObject);

            // Show a new game object for all exists entities.
            ShowGameObjectForGameObjects(gameObject);
        }

        public void RemoveSubscription(IGameObject gameObject)
        {
            if (!gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} does not exists in a region."), LogMessageType.Error);
                return;
            }

            // Hide game objects for the one that left this region.
            HideGameObjectsForGameObject(gameObject.Id);

            // Remove him from region's list.
            gameObjects.Remove(gameObject.Id);

            // Hide the one who left from this region for other game objects.
            HideGameObjectForGameObjects(gameObject.Id);
        }

        public bool HasSubscription(int gameObjectId)
        {
            return gameObjects.ContainsKey(gameObjectId);
        }

        public IEnumerable<IGameObject> GetAllSubscribers()
        {
            return gameObjects.Select(gameObject => gameObject.Value).ToList();
        }

        private void ShowGameObjectsForGameObject(IGameObject gameObject)
        {
            var gameObjectsTemp = gameObjects.Values.Where(gameObjectValue => gameObjectValue.Id != gameObject.Id).ToArray();
            var interestArea = gameObject.Container.GetComponent<InterestArea>().AssertNotNull();

            interestArea.GameObjectsAdded.AssertNotNull().Invoke(gameObjectsTemp); 
        }

        private void HideGameObjectsForGameObject(int hideGameObjectId)
        {
            var gameObjectsTemp = gameObjects.Values.Where(gameObject => gameObject.Id != hideGameObjectId).ToArray();
            var interestArea = gameObjects[hideGameObjectId].Container.GetComponent<InterestArea>().AssertNotNull();

            var removeGameObjects = new List<int>();

            foreach (var gameObject in gameObjectsTemp)
            {
                if (interestArea.GetPublishers().Any(publisher => !publisher.HasSubscription(gameObject.Id)))
                {
                    removeGameObjects.Add(gameObject.Id);
                }
            }

            if (removeGameObjects.Count > 0)
            {
                interestArea.GameObjectsRemoved.AssertNotNull().Invoke(removeGameObjects.ToArray());
            }
        }

        private void ShowGameObjectForGameObjects(IGameObject newGameObject)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Value.Id == newGameObject.Id)
                {
                    continue;
                }

                var interestArea = gameObject.Value.Container.GetComponent<InterestArea>().AssertNotNull();
                interestArea.GameObjectAdded.AssertNotNull().Invoke(newGameObject);
            }
        }

        private void HideGameObjectForGameObjects(int hideGameObjectId)
        {
            foreach (var gameObject in gameObjects.Values)
            {
                var interestArea = gameObject.Container.GetComponent<InterestArea>().AssertNotNull();
                if (!interestArea.GetPublishers().Any(publisher => publisher.HasSubscription(hideGameObjectId)))
                {
                    interestArea.GameObjectRemoved.AssertNotNull().Invoke(hideGameObjectId);
                }
            }
        }
    }
}