using Common.ComponentModel;
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
            player = new PlayerGameObject(id, new IComponent[]
            {
                new AnimationData(),
                new CharacterData(),
                new PresenceMapProvider(),
                new MessageSender(),
                new PositionChangedMessageSender(),
                new AnimationStateChangedMessageSender(),
                new PlayerAttackedMessageSender()
            });
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