using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public class Scene : IScene
    {
        private readonly IRegion[,] regions;
        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        private readonly object locker = new object();

        public Scene(Vector2 sceneSize, Vector2 regionSize)
        {
            var regionsX = (int)(sceneSize.X / regionSize.X);
            var regionsY = (int)(sceneSize.Y / regionSize.Y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.X / 2) + regionSize.X / 2;
            var y = -(sceneSize.Y / 2) + regionSize.Y / 2;

            for (var i = 0; i < regions.GetLength(0); i++)
            {
                for (var j = 0; j < regions.GetLength(1); j++)
                {
                    var regionPositionX = x + (i * regionSize.X);
                    var regionPositionY = y + (j * regionSize.Y);

                    regions[i, j] = new Region(new Rectangle(new Vector2(regionPositionX, regionPositionY),
                        new Vector2(regionSize.X, regionSize.Y)));
                }
            }
        }

        public IGameObject AddGameObject(IGameObject gameObject)
        {
            lock (locker)
            {
                if (gameObjects.ContainsKey(gameObject.Id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A game object with a id #{gameObject.Id} already exists in a scene."), LogMessageType.Warning);
                    return null;
                }

                gameObjects.Add(gameObject.Id, gameObject);

                LogUtils.Log(MessageBuilder.Trace($"A new game object: {gameObject.Name}"));

                return gameObject;
            }
        }

        public void RemoveGameObject(int id)
        {
            lock (locker)
            {
                if (!gameObjects.ContainsKey(id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A game object with a id #{id} does not exists in a scene."), LogMessageType.Warning);
                    return;
                }

                gameObjects[id].RemoveScene();
                gameObjects.Remove(id);

                foreach (var region in regions)
                {
                    if (region.HasSubscription(id))
                    {
                        region.RemoveSubscriptionForOtherOnly(id);
                    }
                }
            }
        }

        public IGameObject GetGameObject(int gameObjectId)
        {
            lock (locker)
            {
                if (gameObjects.TryGetValue(gameObjectId, out var gameObject))
                {
                    return gameObject;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find a game object with id #{gameObjectId}"), LogMessageType.Error);
                return null;
            }
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }
    }
}