using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterGetter : Component<ISceneObject>, ICharacterGetter
    {
        private readonly CharacterFromDatabaseParameters character;

        public CharacterGetter(CharacterFromDatabaseParameters character)
        {
            this.character = character;
        }

        public CharacterFromDatabaseParameters GetCharacter()
        {
            return character;
        }
    }
}