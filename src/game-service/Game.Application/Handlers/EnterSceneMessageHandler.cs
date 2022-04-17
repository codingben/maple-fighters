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
        private readonly ICharacterData characterData;
        private readonly IPresenceMapProvider presenceMapProvider;
        private readonly IMessageSender messageSender;
        private readonly IGameSceneCollection gameSceneCollection;

        public EnterSceneMessageHandler(IGameObject player, IGameSceneCollection gameSceneCollection)
        {
            this.player = player;
            this.gameSceneCollection = gameSceneCollection;

            characterData = player.Components.Get<ICharacterData>();
            presenceMapProvider = player.Components.Get<IPresenceMapProvider>();
            messageSender = player.Components.Get<IMessageSender>();
        }

        public void Handle(EnterSceneMessage message)
        {
            var map = message.Map;
            var mapName = ((Map)map).ToString().ToLower();

            if (gameSceneCollection.TryGet(mapName, out var gameScene))
            {
                if (gameScene.GameObjectCollection.Add(player))
                {
                    var position = gameScene.PlayerSpawnData.GetPosition();
                    var size = gameScene.PlayerSpawnData.GetSize();
                    var direction = gameScene.PlayerSpawnData.GetDirection();

                    player.Transform.SetPosition(position);
                    player.Transform.SetSize(size);

                    var name = message.CharacterName;
                    var type = message.CharacterType;

                    characterData.Name = name;
                    characterData.CharacterType = type;
                    characterData.SpawnDirection = direction;

                    presenceMapProvider?.SetMap(gameScene);

                    SendEnteredSceneMessage();
                }
            }
        }

        private void SendEnteredSceneMessage()
        {
            var messageCode = (byte)MessageCodes.EnteredScene;
            var message = new EnteredSceneMessage()
            {
                GameObjectId = player.Id,
                X = player.Transform.Position.X,
                Y = player.Transform.Position.Y,
                Direction = characterData.SpawnDirection,
                CharacterName = characterData.Name,
                CharacterClass = characterData.CharacterType
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}