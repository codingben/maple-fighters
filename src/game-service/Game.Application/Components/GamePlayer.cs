using Common.MathematicsHelper;
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
            player?.Dispose();
        }

        public void Create()
        {
            // TODO: Refactor. Lobby Spawn Position
            var position = new Vector2(18, -1.86f);

            player = new PlayerGameObject(id, position);
            player.Components.Add(new AnimationData());
            player.Components.Add(new PositionChangedMessageSender());
            player.Components.Add(new AnimationStateChangedMessageSender());
            player.Components.Add(new CharacterData());
        }

        public IGameObject GetPlayer()
        {
            return player;
        }
    }
}