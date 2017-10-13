using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;

namespace Game.Application.GameObjects.Components
{
    internal class CharacterInformationProvider : Component<IGameObject>
    {
        private CharacterInformation characterInformation;

        public CharacterInformationProvider(Character character)
        {
            characterInformation.CharacterName = character.Name;
            characterInformation.CharacterClass = character.CharacterType;
        }

        public string GetCharacterName()
        {
            return characterInformation.CharacterName;
        }

        public CharacterClasses GetCharacterClass()
        {
            return characterInformation.CharacterClass;
        }
    }
}