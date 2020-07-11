using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Components
{
    public class GamePlayer : IGamePlayer
    {
        private readonly int id;
        private IGameObject player;

        public GamePlayer(int id)
        {
            this.id = id;
        }

        public void Dispose()
        {
            Remove();

            player?.Dispose();
        }

        public void Create()
        {
            player = new PlayerGameObject(id);
            player.Components.Add(new AnimationData());
            player.Components.Add(new PositionChangedMessageSender());
            player.Components.Add(new AnimationStateChangedMessageSender());
            player.Components.Add(new CharacterData());
            player.Components.Add(new PresenceMapProvider());
        }

        private void Remove()
        {
            var presenceMapProvider = player?.Components?.Get<IPresenceMapProvider>();
            var map = presenceMapProvider?.GetMap();

            map?.GameObjectCollection?.Remove(id);
        }

        public IGameObject GetPlayer()
        {
            return player;
        }
    }
}