using Game.Application.Components;
using Game.Messages;
using Game.MessageTools;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class EnterSceneMessageHandler : IMessageHandler<EnterSceneMessage>
    {
        private readonly IGameObject player;
        private readonly IPlayerConfigDataProvider playerConfigDataProvider;
        private readonly IPresenceSceneProvider presenceSceneProvider;
        private readonly IMessageSender messageSender;
        private readonly IGameSceneCollection gameSceneCollection;

        public EnterSceneMessageHandler(IGameObject player, IGameSceneCollection gameSceneCollection)
        {
            this.player = player;
            this.gameSceneCollection = gameSceneCollection;

            playerConfigDataProvider = player.Components.Get<IPlayerConfigDataProvider>();
            presenceSceneProvider = player.Components.Get<IPresenceSceneProvider>();
            messageSender = player.Components.Get<IMessageSender>();
        }

        public void Handle(EnterSceneMessage message)
        {
            var map = message.Map;
            var mapName = ((Map)map).ToString().ToLower();

            if (gameSceneCollection.TryGet(mapName, out var gameScene))
            {
                var sceneObjectCollection =
                    gameScene.Components.Get<ISceneObjectCollection>();

                if (sceneObjectCollection.Add(player))
                {
                    var scenePlayerSpawnData =
                        gameScene.Components.Get<IScenePlayerSpawnData>();
                    var position = scenePlayerSpawnData.GetPosition();
                    var size = scenePlayerSpawnData.GetSize();
                    var direction = scenePlayerSpawnData.GetDirection();

                    player.Transform.SetPosition(position);
                    player.Transform.SetSize(size);

                    var name = message.CharacterName;
                    var type = message.CharacterType;
                    var playerConfigData = playerConfigDataProvider.Provide();
                    playerConfigData.CharacterName = name;
                    playerConfigData.CharacterType = type;
                    playerConfigData.CharacterSpawnDirection = direction;

                    presenceSceneProvider?.SetScene(gameScene);

                    SendEnteredSceneMessage();
                }
            }
        }

        private void SendEnteredSceneMessage()
        {
            var playerConfigData = playerConfigDataProvider.Provide();
            var messageCode = (byte)MessageCodes.EnteredScene;
            var message = new EnteredSceneMessage()
            {
                GameObjectId = player.Id,
                X = player.Transform.Position.X,
                Y = player.Transform.Position.Y,
                Direction = playerConfigData.CharacterSpawnDirection,
                CharacterName = playerConfigData.CharacterName,
                CharacterClass = playerConfigData.CharacterType
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}