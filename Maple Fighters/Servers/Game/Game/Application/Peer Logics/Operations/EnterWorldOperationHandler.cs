using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;
using GameObject = Shared.Game.Common.GameObject;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EnterWorldRequestParameters, EnterWorldResponseParameters>
    {
        private readonly int userId;
        private readonly Action<IGameObject> onCharacterSelected;
        private readonly DatabaseCharactersGetter charactersGetter;

        public EnterWorldOperationHandler(int userId, Action<IGameObject> onCharacterSelected)
        {
            this.userId = userId;
            this.onCharacterSelected = onCharacterSelected;

            charactersGetter = Server.Entity.Container.GetComponent<DatabaseCharactersGetter>().AssertNotNull();
        }

        public EnterWorldResponseParameters? Handle(MessageData<EnterWorldRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;

            var character = charactersGetter.GetCharacter(userId, characterIndex);
            if (character == null)
            {
                return new EnterWorldResponseParameters(null, null, false);
            }

            var gameObject = CreatePlayerGameObject(character.Value);
            var sharedGameObject = GetSharedGameObject(gameObject);

            onCharacterSelected.Invoke(gameObject);

            return new EnterWorldResponseParameters(sharedGameObject, character.Value, true);
        }

        private GameObject GetSharedGameObject(IGameObject gameObject)
        {
            const string GAME_OBJECT_NAME = "Local Player";

            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            return new GameObject(gameObject.Id, GAME_OBJECT_NAME, transform.Position.X, transform.Position.Y);
        }

        private IGameObject CreatePlayerGameObject(Character character)
        {
            var playerGameObjectCreator = Server.Entity.Container.GetComponent<PlayerGameObjectCreator>().AssertNotNull();
            var playerGameObject = playerGameObjectCreator.Create(character, Maps.Map_1, new Vector2(10, -5.5f));
            return playerGameObject;
        }
    }
}