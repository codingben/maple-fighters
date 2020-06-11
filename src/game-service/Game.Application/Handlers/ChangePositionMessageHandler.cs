using Game.Application.Objects;
using Common.MathematicsHelper;

namespace Game.Application
{
    public class ChangePositionMessageHandler : IMessageHandler
    {
        private readonly IGameObject player;
        private readonly IGameService gameService;

        public ChangePositionMessageHandler(IGameObject player, IGameService gameService)
        {
            this.player = player;
            this.gameService = gameService;
        }

        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.GetMessage<ChangePlayerPositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;
            var position = new Vector2(x, y);

            player.Transform.SetPosition(position);

            ChangePositionForNearbyGameObjects(position);
        }

        private void ChangePositionForNearbyGameObjects(Vector2 position)
        {
            // TODO: Refactor this: collection.GetAllNearbyGameObjects()
            var nearbyGameObjects = new int[0];

            foreach (var id in nearbyGameObjects)
            {
                var message = new PositionChangedMessage()
                {
                    GameObjectId = id,
                    X = position.X,
                    Y = position.Y
                };
                var data = MessageUtils.ToMessage(message);

                gameService.SendMessage(data, id);
            }
        }
    }
}