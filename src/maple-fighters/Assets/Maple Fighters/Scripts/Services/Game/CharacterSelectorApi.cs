using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Network.Scripts;

namespace Scripts.Services.Game
{
    internal class CharacterSelectorApi : NetworkApi<CharacterOperations, EmptyEventCode>, ICharacterSelectorApi
    {
        internal CharacterSelectorApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    CharacterOperations.CreateCharacter,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<CreateCharacterResponseParameters>(
                        yield,
                        id);
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
            IYield yield,
            RemoveCharacterRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    CharacterOperations.RemoveCharacter,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<RemoveCharacterResponseParameters>(
                        yield,
                        id);
        }

        public async Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield,
            ValidateCharacterRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    CharacterOperations.ValidateCharacter,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<ValidateCharacterResponseParameters>(
                        yield,
                        id);
        }

        public async Task<GetCharactersResponseParameters> GetCharactersAsync(IYield yield)
        {
            var id =
                OperationRequestSender.Send(
                    CharacterOperations.GetCharacters,
                    new EmptyParameters(),
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<GetCharactersResponseParameters>(
                        yield,
                        id);
        }
    }
}