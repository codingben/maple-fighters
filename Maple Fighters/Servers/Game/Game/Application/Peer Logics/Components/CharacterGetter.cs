using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal class CharacterGetter : Component, ICharacterGetter
    {
        private readonly Character character; // TODO: CharacterInformation?

        public CharacterGetter(Character character)
        {
            this.character = character;
        }

        public Character GetCharacter()
        {
            return character;
        }
    }
}