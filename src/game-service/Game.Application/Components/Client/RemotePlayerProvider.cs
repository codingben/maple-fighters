using Game.Application.Objects;

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

            gameObject?.Dispose();
        }

        public bool AddToPresenceScene()
        {
            var map =
                gameObject.Components.Get<IPresenceMapProvider>().GetMap();
            var sceneObjectCollection =
                map.Components.Get<ISceneObjectCollection>();

            return sceneObjectCollection.Add(gameObject);
        }

        public bool RemoveFromPresenceScene()
        {
            var map =
                gameObject.Components.Get<IPresenceMapProvider>().GetMap();
            var sceneObjectCollection =
                map.Components.Get<ISceneObjectCollection>();

            return sceneObjectCollection.Remove(id);
        }

        public IGameObject Provide()
        {
            return gameObject;
        }
    }
}