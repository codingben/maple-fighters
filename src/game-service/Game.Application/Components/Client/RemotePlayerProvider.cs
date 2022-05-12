using Game.Application.Objects;

namespace Game.Application.Components
{
    public class RemotePlayerProvider : ComponentBase, IRemotePlayerProvider
    {
        private readonly IGameObject gameObject;

        public RemotePlayerProvider(int id)
        {
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