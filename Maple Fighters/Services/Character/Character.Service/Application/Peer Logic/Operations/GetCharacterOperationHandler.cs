using Character.Server.Common;
using CharacterService.Application.Components.Interfaces;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharacterService.Application.PeerLogics.Operations
{
    internal class GetCharacterOperationHandler : IOperationRequestHandler<GetCharacterRequestParametersEx, GetCharacterResponseParameters>
    {
        private readonly IDatabaseCharacterGetter characterGetter;

        public GetCharacterOperationHandler()
        {
            characterGetter = Server.Components.GetComponent<IDatabaseCharacterGetter>().AssertNotNull();
        }

        public GetCharacterResponseParameters? Handle(MessageData<GetCharacterRequestParametersEx> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var characterIndex = messageData.Parameters.CharacterIndex;
            var character = characterGetter.GetCharacter(userId, characterIndex);
            return new GetCharacterResponseParameters(character.GetValueOrDefault());
        }
    }
}