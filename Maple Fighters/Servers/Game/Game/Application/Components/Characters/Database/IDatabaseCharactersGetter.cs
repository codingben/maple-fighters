using System.Collections.Generic;
using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface IDatabaseCharactersGetter : IExposableComponent
    {
        IEnumerable<CharacterFromDatabaseParameters> GetCharacters(int userId);
        CharacterFromDatabaseParameters? GetCharacter(int userId, int characterIndex);
    }
}