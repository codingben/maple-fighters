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
                return new CharacterParameters(charactersTableDefinition.Name, CharacterClasses.Arrow, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Knight.ToString())
            {
                return new CharacterParameters(charactersTableDefinition.Name, CharacterClasses.Knight, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Wizard.ToString())
            {
                return new CharacterParameters(charactersTableDefinition.Name, CharacterClasses.Wizard, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            LogUtils.Log(MessageBuilder.Trace($"Can not get character type of user id #{charactersTableDefinition.UserId}"));
            return new CharacterParameters { HasCharacter = false, Index = (CharacterIndex)charactersTableDefinition.CharacterIndex };
        }
    }
}