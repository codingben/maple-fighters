using Game.Application.Components;
using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class EnterSceneMessageHandler : IMessageHandler
    {
        private readonly IGameObject player;
        private readonly IGameSceneCollection gameSceneCollection;
        private readonly ICharacterData characterData;
        private readonly IPresenceMapProvider presenceMapProvider;
        private readonly IMessageSender messageSender;

        public EnterSceneMessageHandler(IGameObject player, IGameSceneCollection gameSceneCollection)
        {
            this.player = player;
            this.gameSceneCollection = gameSceneCollection;
            this.characterData = player.Components.Get<ICharacterData>();
            this.presenceMapProvider = player.Components.Get<IPresenceMapProvider>();
            this.messageSender = player.Components.Get<IMessageSender>();
        }

        public void Handle(byte[] rawData)
        {
            var message =
                MessageUtils.DeserializeMessage<EnterSceneMessage>(rawData);
            var map = message.Map;
            var name = message.CharacterName;
            var characterType = message.CharacterType;

            characterData.SetName(name);
            characterData.SetCharacterType(characterType);

            if (gameSceneCollection.TryGet((Map)map, out var gameScene))
            {
                // Set's player position
                var position = gameScene.GamePlayerSpawnData.GetSpawnPosition();
                player.Transform.SetPosition(position);

                // Set's map
                presenceMapProvider.SetMap(gameScene);

                // Adds to this game scene
                gameScene.GameObjectCollection.Add(player);
            }

            SendEnteredSceneMessage();
        }

        private void SendEnteredSceneMessage()
        {
            var messageCode = (byte)MessageCodes.EnteredScene;
            var message = new EnteredSceneMessage()
            {
                GameObjectId = player.Id,
                SpawnPositionData = new SpawnPositionData
                {
                    X = player.Transform.Position.X,
                    Y = player.Transform.Position.Y
                }
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}