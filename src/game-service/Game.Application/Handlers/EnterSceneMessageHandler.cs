using Game.Application.Components;
using Game.Messages;
using Game.MessageTools;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Common.MathematicsHelper;

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
                    var name = message.CharacterName;
                    var characterType = message.CharacterType;
                    var direction = gameScene.SpawnData.Direction;
                    var position = gameScene.SpawnData.Position;
                    var size = gameScene.SpawnData.Size;

                    SetCharacterData(name, characterType, direction);
                    SetPresenceScene(gameScene);
                    SetTransform(position, size);
                    SetBody(gameScene);
                    SendEnteredSceneMessage();
                }
            }
        }

        private void SetCharacterData(string name, byte type, float direction)
        {
            characterData.Name = name;
            characterData.CharacterType = type;
            characterData.SpawnDirection = direction;
        }

        private void SetPresenceScene(IGameScene scene)
        {
            presenceMapProvider?.SetMap(scene);
        }

        private void SetTransform(Vector2 position, Vector2 size)
        {
            player.Transform.SetPosition(position);
            player.Transform.SetSize(size);
        }

        private void SetBody(IGameScene scene)
        {
            var gameObject = player as PlayerGameObject;
            if (gameObject != null)
            {
                var bodyData = gameObject.CreateBodyData();
                scene.PhysicsWorldManager.AddBody(bodyData);
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