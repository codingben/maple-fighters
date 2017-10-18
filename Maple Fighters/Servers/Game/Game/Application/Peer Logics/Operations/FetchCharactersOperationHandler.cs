using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class FetchCharactersOperationHandler : IOperationRequestHandler<EmptyParameters, FetchCharactersResponseParameters>
    {
        private readonly int userId;
        private readonly IDatabaseCharactersGetter databaseCharactersGetter;

        public FetchCharactersOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharactersGetter = Server.Entity.Container.GetComponent<IDatabaseCharactersGetter>().AssertNotNull();
        }

        public FetchCharactersResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characters = databaseCharactersGetter.GetCharacters(userId).ToArray();
            return new FetchCharactersResponseParameters(characters);
        }
    }
}