using System;
using System.Threading.Tasks;
using Characters.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ValidateCharacterOperationHandler : IAsyncOperationRequestHandler<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly Action<CharacterFromDatabaseParameters?> onCharacterSelected;
        private readonly ICharactersServiceAPI charactersServiceApi;

        public ValidateCharacterOperationHandler(int userId, Action<CharacterFromDatabaseParameters?> onCharacterSelected)
        {
            this.userId = userId;
            this.onCharacterSelected = onCharacterSelected;

            charactersServiceApi = Server.Components.GetComponent<ICharactersServiceAPI>().AssertNotNull();
        }

        public Task<ValidateCharacterResponseParameters?> Handle(IYield yield, MessageData<ValidateCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;
            var parameters = new FetchCharacterRequestParameters(userId, characterIndex);
            return GetCharacter(yield, parameters);
        }

        public async Task<ValidateCharacterResponseParameters?> GetCharacter(IYield yield, FetchCharacterRequestParameters parameters)
        {
            var character = await charactersServiceApi.SendYieldOperation<FetchCharacterRequestParameters, FetchCharacterResponseParameters>
                (yield, (byte)CharactersServiceOperations.FetchCharacter, parameters);

            onCharacterSelected.Invoke(character.Character);
            return new ValidateCharacterResponseParameters(character.Character.HasValue ? ValidateCharacterStatus.Ok : ValidateCharacterStatus.Wrong);
        }
    }
}