using System.Threading.Tasks;
using Character.Server.Common;
using Game.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Game.Application.PeerLogic.Operations
{
    internal class GetCharactersOperationHandler : IAsyncOperationRequestHandler<EmptyParameters, GetCharactersResponseParameters>
    {
        private readonly int userId;
        private readonly ICharacterServiceAPI characterServiceAPI;

        public GetCharactersOperationHandler(int userId)
        {
            this.userId = userId;

            characterServiceAPI = Server.Components.GetComponent<ICharacterServiceAPI>().AssertNotNull();
        }

        public Task<GetCharactersResponseParameters?> Handle(IYield yield, MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = new GetCharactersRequestParameters(userId);
            return GetCharacters(yield, parameters);
        }

        private async Task<GetCharactersResponseParameters?> GetCharacters(IYield yield, GetCharactersRequestParameters parameters)
        {
            var responseParameters = await characterServiceAPI.GetCharacters(yield, parameters);
            return responseParameters;
        }
    }
}