using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ValidateCharacterOperationHandler : IOperationRequestHandler<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly Action<Character> onCharacterSelected;
        private readonly IDatabaseCharactersGetter charactersGetter;

        public ValidateCharacterOperationHandler(int userId, Action<Character> onCharacterSelected)
        {
            this.userId = userId;
            this.onCharacterSelected = onCharacterSelected;

            charactersGetter = Server.Entity.Container.GetComponent<IDatabaseCharactersGetter>().AssertNotNull();
        }

        public ValidateCharacterResponseParameters? Handle(MessageData<ValidateCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;

            var character = charactersGetter.GetCharacter(userId, characterIndex);
            if (character == null)
            {
                return new ValidateCharacterResponseParameters(ValidateCharacterStatus.Wrong);
            }

            onCharacterSelected.Invoke(character.Value);
            return new ValidateCharacterResponseParameters(ValidateCharacterStatus.Ok);
        }
    }
}