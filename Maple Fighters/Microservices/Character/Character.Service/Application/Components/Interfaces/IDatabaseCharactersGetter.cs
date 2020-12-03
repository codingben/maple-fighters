using System.Collections.Generic;
using Game.Common;

namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharactersGetter
    {
        IEnumerable<CharacterParameters> GetCharacters(int userId);
    }
}