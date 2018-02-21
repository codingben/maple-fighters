using Characters.Client.Common;
using CommonTools.Log;
using Database.Common.TablesDefinition;

namespace CharactersService.Application.Components
{
    internal static class Utils
    {
        internal static CharacterFromDatabaseParameters GetCharacterParameters(CharactersTableDefinition charactersTableDefinition)
        {
            if (charactersTableDefinition.CharacterType == CharacterClasses.Arrow.ToString())
            {
                return new CharacterFromDatabaseParameters(charactersTableDefinition.Name, CharacterClasses.Arrow, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Knight.ToString())
            {
                return new CharacterFromDatabaseParameters(charactersTableDefinition.Name, CharacterClasses.Knight, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Wizard.ToString())
            {
                return new CharacterFromDatabaseParameters(charactersTableDefinition.Name, CharacterClasses.Wizard, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            LogUtils.Log(MessageBuilder.Trace($"Can not get character type of user id #{charactersTableDefinition.UserId}"));
            return new CharacterFromDatabaseParameters { HasCharacter = false, Index = (CharacterIndex)charactersTableDefinition.CharacterIndex };
        }
    }
}