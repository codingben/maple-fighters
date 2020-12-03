using ComponentModel.Common;
using Game.Application.GameObjects.Components.Interfaces;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.GameObjects.Components
{
    internal class CharacterParametersGetter : Component<ISceneObject>, ICharacterParametersGetter
    {
        private readonly CharacterParameters character;

        public CharacterParametersGetter(CharacterParameters character)
        {
            this.character = character;
        }

        public CharacterParameters GetCharacter()
        {
            return character;
        }
    }
}