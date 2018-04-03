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
    internal class RemoveCharacterOperationHandler : IAsyncOperationRequestHandler<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly ICharacterServiceAPI characterServiceAPI;

        public RemoveCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            characterServiceAPI = Server.Components.GetComponent<ICharacterServiceAPI>().AssertNotNull();
        }

        public Task<RemoveCharacterResponseParameters?> Handle(IYield yield, MessageData<RemoveCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = new RemoveCharacterRequestParametersEx(userId, messageData.Parameters.CharacterIndex);
            return RemoveCharacter(yield, parameters);
        }

        private async Task<RemoveCharacterResponseParameters?> RemoveCharacter(IYield yield, RemoveCharacterRequestParametersEx parameters)
        {
            var responseParameters = await characterServiceAPI.RemoveCharacter(yield, parameters);
            return responseParameters;
        }
    }
}