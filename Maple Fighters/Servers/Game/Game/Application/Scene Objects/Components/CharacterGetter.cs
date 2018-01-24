using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterGetter : Component<ISceneObject>, ICharacterGetter
    {
        private readonly CharacterFromDatabase character;

        public CharacterGetter(CharacterFromDatabase character)
        {
            this.character = character;
        }

        public CharacterFromDatabase GetCharacter()
        {
            return character;
        }
    }
}