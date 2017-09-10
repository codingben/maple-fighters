using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        private readonly int Id;

        public Rectangle Area { get; }

        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        public Region(Rectangle rectangle)
        {
            Area = rectangle;

            Id = Server.Entity.Container.GetComponent<IdGenerator>().GenerateId();
        }

        public void AddSubscription(IGameObject gameObject)
        {
            if (gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            gameObjects.Add(gameObject.Id, gameObject);

            LogUtils.Log(MessageBuilder.Trace($"Region Id: {Id} Game Objects Length: {gameObjects.Count}"));

            if (gameObjects.Count >= 2)
            {
                ShowGameObjectsForGameObject(gameObject); // Show all exists entities for a new game object.
                ShowGameObjectForGameObjects(gameObject); // Show a new game object for all exists entities.
            }
        }

        public void RemoveSubscription(IGameObject gameObject)
        {
            if (!gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} does not exists in a region."), LogMessageType.Error);
                return;
            }

            if (gameObjects.Count >= 2)
            {
                HideGameObjectsForGameObject(gameObject.Id);
                HideGameObjectForGameObjects(gameObject.Id);
            }

            gameObjects.Remove(gameObject.Id);

            LogUtils.Log(MessageBuilder.Trace($"Region Id: {Id} Game Objects Length: {gameObjects.Count}"));
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
            var gameObjectsTemp = gameObjects.Keys.Where(gameObjectId => gameObjectId != hideGameObjectId).ToArray();
            var interestArea = gameObjects[hideGameObjectId].Container.GetComponent<InterestArea>().AssertNotNull();

            interestArea.GameObjectsRemoved.AssertNotNull().Invoke(gameObjectsTemp);
        }

        private void ShowGameObjectForGameObjects(IGameObject newGameObject)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key == newGameObject.Id)
                {
                    continue;
                }

                var interestArea = gameObject.Value.Container.GetComponent<InterestArea>().AssertNotNull();
                interestArea.GameObjectAdded.AssertNotNull().Invoke(newGameObject);
            }
        }

        private void HideGameObjectForGameObjects(int hideGameObjectId)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key == hideGameObjectId)
                {
                    continue;
                }

                var interestArea = gameObject.Value.Container.GetComponent<InterestArea>().AssertNotNull();
                interestArea.GameObjectRemoved.AssertNotNull().Invoke(hideGameObjectId);
            }
        }
    }
}