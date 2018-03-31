using ComponentModel.Common;
using Game.Common;
using Game.InterestManagement;

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