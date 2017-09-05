using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Scene : IScene
    {
        private readonly IRegion[,] regions;
        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        private readonly object locker = new object();

        public Scene(Vector2 sceneSize, Vector2 regionsSize)
        {
            var regionsX = (int)(sceneSize.X / regionsSize.X);
            var regionsY = (int)(sceneSize.Y / regionsSize.Y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.X / 2) + regionsSize.X / 2;
            var y = -(sceneSize.Y / 2) + regionsSize.Y / 2;

            for (var i = 0; i < regions.GetLength(0); i++)
            {
                for (var j = 0; j < regions.GetLength(1); j++)
                {
                    var regionPositionX = x + (i * regionsSize.X);
                    var regionPositionY = y + (j * regionsSize.Y);

                    regions[i, j] = new Region(new Rectangle(new Vector2(regionPositionX, regionPositionY),
                        new Vector2(regionsSize.X, regionsSize.Y)));
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

                gameObject.Scene = this;
                gameObjects.Add(gameObject.Id, gameObject);

                return gameObject;
            }
        }

        public void RemoveGameObject(IGameObject gameObject)
        {
            lock (locker)
            {
                if (!gameObjects.ContainsKey(gameObject.Id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A game object with a id #{gameObject.Id} does not exists in a scene."), LogMessageType.Warning);
                    return;
                }

                gameObjects.Remove(gameObject.Id);
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