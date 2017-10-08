using System.Collections.Generic;
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
        private readonly DatabaseCharactersGetter databaseCharactersGetter;

        public FetchCharactersOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharactersGetter = Server.Entity.Container.GetComponent<DatabaseCharactersGetter>().AssertNotNull();
        }

        public FetchCharactersResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var charactersFromDatabase = databaseCharactersGetter.GetCharacters(userId);

            var characters = new List<Character>(3);
            characters.AddRange(from character in charactersFromDatabase where character != null select character.Value);

            return new FetchCharactersResponseParameters(characters.ToArray());
        }
    }
}