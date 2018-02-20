using System.Linq;
using CharactersService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace CharactersService.Application.PeerLogics.Operations
{
    internal class FetchCharactersOperationHandler : IOperationRequestHandler<EmptyParameters, FetchCharactersResponseParameters>
    {
        private readonly int userId;
        private readonly IDatabaseCharactersGetter databaseCharactersGetter;

        public FetchCharactersOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharactersGetter = Server.Components.GetComponent<IDatabaseCharactersGetter>().AssertNotNull();
        }

        public FetchCharactersResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characters = databaseCharactersGetter.GetCharacters(userId).ToArray();
            return new FetchCharactersResponseParameters(characters);
        }
    }
}