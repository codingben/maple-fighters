using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;
using GameObject = Shared.Game.Common.GameObject;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EnterWorldRequestParameters, EnterWorldResponseParameters>
    {
        private readonly IGameObject gameObject;
        private readonly int userId;
        private readonly DatabaseCharactersGetter charactersGetter;

        public EnterWorldOperationHandler(IGameObject gameObject, int userId)
        {
            this.gameObject = gameObject;
            this.userId = userId;

            charactersGetter = Server.Entity.Container.GetComponent<DatabaseCharactersGetter>().AssertNotNull();
        }

        public EnterWorldResponseParameters? Handle(MessageData<EnterWorldRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;

            var character = charactersGetter.GetCharacter(userId, characterIndex);
            if (character == null)
            {
                return new EnterWorldResponseParameters();
            }

            var gameObject = GetGameObject();
            return new EnterWorldResponseParameters(gameObject, character.Value);
        }

        private GameObject GetGameObject()
        {
            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            return new GameObject(gameObject.Id, "Local Player", transform.Position.X, transform.Position.Y);
        }
    }
}