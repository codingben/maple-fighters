using Characters.Common;
using CharactersService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharactersService.Application.PeerLogics.Operations
{
    internal class FetchCharacterOperationHandler : IOperationRequestHandler<FetchCharacterRequestParameters, FetchCharacterResponseParameters>
    {
        private readonly IDatabaseCharactersGetter charactersGetter;

        public FetchCharacterOperationHandler()
        {
            charactersGetter = Server.Components.GetComponent<IDatabaseCharactersGetter>().AssertNotNull();
        }

        public FetchCharacterResponseParameters? Handle(MessageData<FetchCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var characterIndex = messageData.Parameters.CharacterIndex;
            var character = charactersGetter.GetCharacter(userId, characterIndex);
            return new FetchCharacterResponseParameters(character);
        }
    }
}