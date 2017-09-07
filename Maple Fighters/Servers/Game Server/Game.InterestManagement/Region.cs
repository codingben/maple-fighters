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

            ShowGameObjectsForGameObject(gameObject); // Show all exists entities for a new game object.
            ShowGameObjectForGameObjects(gameObject); // Show a new game object for all exists entities.
        }

        public void RemoveSubscription(IGameObject gameObject)
        {
            if (!gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} does not exists in a region."), LogMessageType.Error);
                return;
            }

            if (gameObjects.Count > 0)
            {
                HideGameObjectsForGameObject(gameObject.Id);
            }

            HideGameObjectForGameObjects(gameObject.Id);

            gameObjects.Remove(gameObject.Id);
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
            var gameObjectsTemp = gameObjects.Values.Where(gameObjectValue => gameObjectValue.Id != gameObject.Id).ToList();
            gameObject.Components.GetComponent<InterestArea>().AssertNotNull().GameObjectsAdded?.Invoke(gameObjectsTemp.ToArray()); 
        }

        private void HideGameObjectsForGameObject(int id)
        {
            var gameObjectsTemp = gameObjects.Keys.Where(gameObjectId => gameObjectId != id).ToList();
            gameObjects[id].Components.GetComponent<InterestArea>().AssertNotNull().GameObjectsRemoved?.Invoke(gameObjectsTemp.ToArray());
        }

        private void ShowGameObjectForGameObjects(IGameObject newGameObject)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key == newGameObject.Id)
                {
                    continue;
                }

                gameObject.Value.Components.GetComponent<InterestArea>().AssertNotNull().GameObjectAdded?.Invoke(newGameObject);
            }
        }

        private void HideGameObjectForGameObjects(int id)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key == id)
                {
                    continue;
                }

                gameObject.Value.Components.GetComponent<InterestArea>().AssertNotNull().GameObjectRemoved?.Invoke(id);
            }
        }
    }
}