using System;
using Game.Application.Components;
using Game.Application.Objects.Components;
using Game.Messages;

namespace Game.Application.Objects
{
    public class PresenceSceneProvider : ComponentBase, IPresenceSceneProvider
    {
        public event Action<IGameScene> SceneChanged;

        private IGameScene gameScene;
        private IProximityChecker proximityChecker;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();

            SetRegion();
        }

        protected override void OnRemoved()
        {
            RemoveFromPresenceScene();
            RemoveGameObjectForOtherObjects();
        }

        public void SetScene(IGameScene gameScene)
        {
            this.gameScene = gameScene;

            SetRegion();

            SceneChanged?.Invoke(gameScene);
        }

        public IGameScene GetScene()
        {
            return gameScene;
        }

        private void SetRegion()
        {
            var sceneRegionCreator =
                gameScene?.Components.Get<ISceneRegionCreator>();
            if (sceneRegionCreator != null)
            {
                var region = sceneRegionCreator.GetRegion();

                proximityChecker.SetMatrixRegion(region);
            }
        }

        private void RemoveFromPresenceScene()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();
            var sceneObjectCollection =
                gameScene.Components.Get<ISceneObjectCollection>();
            var id = gameObject.Id;

            sceneObjectCollection.Remove(id);
        }

        private void RemoveGameObjectForOtherObjects()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var id = gameObjectGetter.Get().Id;
            var messageCode = (byte)MessageCodes.GameObjectsRemoved;
            var message = new GameObjectsRemovedMessage()
            {
                GameObjectIds = new int[]
                {
                    id
                }
            };
            var sceneObjectCollection =
                gameScene.Components.Get<ISceneObjectCollection>();
            var gameObjects = sceneObjectCollection.GetAll();

            foreach (var gameObject in gameObjects)
            {
                var messageSender =
                    gameObject.Components.Get<IMessageSender>();
                messageSender?.SendMessage(messageCode, message);
            }
        }
    }
}