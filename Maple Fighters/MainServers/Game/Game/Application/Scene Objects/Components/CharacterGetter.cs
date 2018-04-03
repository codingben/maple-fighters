using ComponentModel.Common;
using Game.Application.SceneObjects.Components.Interfaces;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterGetter : Component<ISceneObject>, ICharacterGetter
    {
        private readonly CharacterParameters character;

        public CharacterGetter(CharacterParameters character)
        {
            this.character = character;
        }

        public CharacterParameters GetCharacter()
        {
            return character;
        }
    }
}