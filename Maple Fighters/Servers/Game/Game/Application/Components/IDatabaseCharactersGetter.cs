using System.Collections.Generic;
using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface IDatabaseCharactersGetter : IExposableComponent
    {
        IEnumerable<Character> GetCharacters(int userId);
        Character? GetCharacter(int userId, int characterIndex);
    }
}