using Character.Server.Common;
using CharacterService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharacterService.Application.PeerLogics.Operations
{
    internal class GetCharacterOperationHandler : IOperationRequestHandler<GetCharacterRequestParameters, GetCharacterResponseParameters>
    {
        private readonly IDatabaseCharacterGetter characterGetter;

        public GetCharacterOperationHandler()
        {
            characterGetter = Server.Components.GetComponent<IDatabaseCharacterGetter>().AssertNotNull();
        }

        public GetCharacterResponseParameters? Handle(MessageData<GetCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var characterIndex = messageData.Parameters.CharacterIndex;
            var character = characterGetter.GetCharacter(userId, characterIndex);
            return new GetCharacterResponseParameters(character.GetValueOrDefault());
        }
    }
}