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

            if (gameSceneCollection.TryGet((Map)map, out var gameScene))
            {
                if (gameScene.GameObjectCollection.Add(player))
                {
                    // Set character data
                    var name = message.CharacterName;
                    var characterType = message.CharacterType;
                    var spawnDirection = gameScene.SpawnData.Direction;

                    characterData.Name = name;
                    characterData.CharacterType = characterType;
                    characterData.SpawnDirection = spawnDirection;

                    // Set player transform
                    var position = gameScene.SpawnData.Position;
                    var size = gameScene.SpawnData.Size;

                    player.Transform.SetPosition(position);
                    player.Transform.SetSize(size);

                    // Set player map
                    presenceMapProvider.SetMap(gameScene);

                    // Add player body
                    var gameObject = player as PlayerGameObject;
                    if (gameObject != null)
                    {
                        var bodyData = gameObject.CreateBodyData();
                        gameScene.PhysicsWorldManager.AddBody(bodyData);
                    }

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