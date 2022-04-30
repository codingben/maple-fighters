using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.Messages;

namespace Game.Application.Components
{
    public class RemotePlayerProvider : ComponentBase, IRemotePlayerProvider
    {
        private int id;
        private IGameObject gameObject;

        protected override void OnAwake()
        {
            var webSocketConnectionProvider =
                Components.Get<IWebSocketConnectionProvider>();
            id = webSocketConnectionProvider.ProvideId();
            gameObject = new PlayerGameObject(id, name: "RemotePlayer");
        }

        protected override void OnRemoved()
        {
            RemoveFromPresenceScene();
            RemovePlayerForOtherPlayers();

            gameObject?.Dispose();
        }

        public bool AddToPresenceScene()
        {
            var scene =
                gameObject.Components.Get<IPresenceSceneProvider>().GetScene();
            var sceneObjectCollection =
                scene.Components.Get<ISceneObjectCollection>();

            return sceneObjectCollection.Add(gameObject);
        }

        public bool RemoveFromPresenceScene()
        {
            var scene =
                gameObject.Components.Get<IPresenceSceneProvider>().GetScene();
            var sceneObjectCollection =
                scene.Components.Get<ISceneObjectCollection>();

            return sceneObjectCollection.Remove(id);
        }

        private void RemovePlayerForOtherPlayers()
        {
            var messageSender = gameObject.Components.Get<IMessageSender>();
            var messageCode = (byte)MessageCodes.GameObjectsRemoved;
            var message = new GameObjectsRemovedMessage()
            {
                GameObjectIds = new int[]
                {
                    gameObject.Id
                }
            };

            messageSender.SendMessageToNearbyGameObjects(messageCode, message);
        }

        public IGameObject Provide()
        {
            return gameObject;
        }
    }
}