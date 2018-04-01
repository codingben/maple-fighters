using System.Collections.Generic;
using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharactersGetter
    {
        IEnumerable<CharacterParameters> GetCharacters(int userId);
    }
}