using System.Threading.Tasks;
using Character.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Game.Application.PeerLogic.Operations
{
    internal class CreateCharacterOperationHandler : IAsyncOperationRequestHandler<CreateCharacterRequestParameters, CreateCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly ICharacterServiceAPI characterServiceAPI;

        public CreateCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            characterServiceAPI = Server.Components.GetComponent<ICharacterServiceAPI>().AssertNotNull();
        }
        
        public Task<CreateCharacterResponseParameters?> Handle(IYield yield, MessageData<CreateCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = new CreateCharacterRequestParametersEx(userId, messageData.Parameters.CharacterClass, messageData.Parameters.Name, messageData.Parameters.Index);
            return CreateCharacter(yield, parameters);
        }

        private async Task<CreateCharacterResponseParameters?> CreateCharacter(IYield yield, CreateCharacterRequestParametersEx parameters)
        {
            var responseParameters = await characterServiceAPI.CreateCharacter(yield, parameters);
            return responseParameters;
        }
    }
}