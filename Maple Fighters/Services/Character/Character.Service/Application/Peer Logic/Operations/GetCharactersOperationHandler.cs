using System.Linq;
using Character.Server.Common;
using CharacterService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharacterService.Application.PeerLogics.Operations
{
    internal class GetCharactersOperationHandler : IOperationRequestHandler<GetCharactersRequestParameters, GetCharactersResponseParameters>
    {
        private readonly IDatabaseCharactersGetter databaseCharactersGetter;

        public GetCharactersOperationHandler()
        {
            databaseCharactersGetter = Server.Components.GetComponent<IDatabaseCharactersGetter>().AssertNotNull();
        }

        public GetCharactersResponseParameters? Handle(MessageData<GetCharactersRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var characters = databaseCharactersGetter.GetCharacters(userId).ToArray();
            return new GetCharactersResponseParameters(characters);
        }
    }
}