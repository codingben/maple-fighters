using System.Threading.Tasks;
using Authorization.Client.Common;
using Character.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Scripts.Utils;

namespace Scripts.Services
{
    public sealed class CharacterService : ServiceBase<CharacterOperations, EmptyEventCode>, ICharacterService
    {
        private AuthorizationStatus authorizationStatus = AuthorizationStatus.Failed;

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            GoBackToLogin();
        }

        private void GoBackToLogin()
        {
            if (authorizationStatus != AuthorizationStatus.Failed)
            {
                return;
            }

            UI.Utils.ShowNotice("Authorization with character service failed.", LoadedObjects.DestroyAll);
        }

        public async Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            var responseParameters = await ServerPeerHandler.SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>
                (yield, (byte)CharacterOperations.Authorize, parameters, MessageSendOptions.DefaultReliable());
            authorizationStatus = responseParameters.Status;
            return responseParameters;
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<CreateCharacterRequestParameters, CreateCharacterResponseParameters>
                (yield, (byte)CharacterOperations.CreateCharacter, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>
                (yield, (byte)CharacterOperations.RemoveCharacter, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<GetCharactersResponseParameters> GetCharacters(IYield yield)
        {
            var parameters = new EmptyParameters();
            return await ServerPeerHandler.SendOperation<EmptyParameters, GetCharactersResponseParameters>
                (yield, (byte)CharacterOperations.GetCharacters, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}