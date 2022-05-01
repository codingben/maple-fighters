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

            playerConfigDataProvider =
                player.Components.Get<IPlayerConfigDataProvider>();
            presenceSceneProvider =
                player.Components.Get<IPresenceSceneProvider>();
            messageSender =
                player.Components.Get<IMessageSender>();
        }

        public void Handle(EnterSceneMessage message)
        {
            var map = message.Map;
            var name = message.CharacterName;
            var type = message.CharacterType;
            var mapName = ((Map)map).ToString().ToLower();

            if (gameSceneCollection.TryGet(mapName, out var gameScene))
            {
                var sceneObjectCollection =
                    gameScene.Components.Get<ISceneObjectCollection>();
                if (sceneObjectCollection.Add(player))
                {
                    SetPlayerTransform(gameScene);
                    SetPlayerConfigData(gameScene, name, type);
                    SetPlayerPresenceScene(gameScene);
                    SendEnteredSceneMessage();
                }
            }
        }

        private void SetPlayerTransform(IGameScene gameScene)
        {
            var scenePlayerSpawnData =
                gameScene.Components.Get<IScenePlayerSpawnData>();
            var position = scenePlayerSpawnData.GetPosition();
            var size = scenePlayerSpawnData.GetSize();

            player.Transform.SetPosition(position);
            player.Transform.SetSize(size);
        }

        private void SetPlayerConfigData(IGameScene gameScene, string name, byte type)
        {
            var scenePlayerSpawnData =
                gameScene.Components.Get<IScenePlayerSpawnData>();
            var direction = scenePlayerSpawnData.GetDirection();
            var playerConfigData = playerConfigDataProvider.Provide();

            playerConfigData.CharacterName = name;
            playerConfigData.CharacterType = type;
            playerConfigData.CharacterSpawnDirection = direction;
        }

        private void SetPlayerPresenceScene(IGameScene gameScene)
        {
            presenceSceneProvider.SetScene(gameScene);
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