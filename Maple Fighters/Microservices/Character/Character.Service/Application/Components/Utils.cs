using CommonTools.Log;
using Database.Common.TablesDefinition;
using Game.Common;

namespace CharacterService.Application.Components
{
    internal static class Utils
    {
        internal static CharacterParameters GetCharacterParameters(CharactersTableDefinition charactersTableDefinition)
        {
            if (charactersTableDefinition.CharacterType == CharacterClasses.Arrow.ToString())
            {
                return ToParameters(CharacterClasses.Arrow);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Knight.ToString())
            {
                return ToParameters(CharacterClasses.Knight);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Wizard.ToString())
            {
                return ToParameters(CharacterClasses.Wizard);
            }

            LogUtils.Log(MessageBuilder.Trace($"Can not get character type of user id #{charactersTableDefinition.UserId}"));
            return new CharacterParameters { HasCharacter = false, Index = (CharacterIndex)charactersTableDefinition.CharacterIndex };

            CharacterParameters ToParameters(CharacterClasses characterClass)
            {
                return new CharacterParameters(charactersTableDefinition.Name, characterClass,
                    (CharacterIndex)charactersTableDefinition.CharacterIndex, (Maps)charactersTableDefinition.LastMap);
            }
        }
    }
}