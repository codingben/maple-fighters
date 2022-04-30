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
            gameObject?.Dispose();
        }

        public IGameObject Provide()
        {
            return gameObject;
        }
    }
}