using System.Collections.Generic;
using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface IDatabaseCharactersGetter : IExposableComponent
    {
        IEnumerable<CharacterFromDatabase> GetCharacters(int userId);
        CharacterFromDatabase? GetCharacter(int userId, int characterIndex);
    }
}