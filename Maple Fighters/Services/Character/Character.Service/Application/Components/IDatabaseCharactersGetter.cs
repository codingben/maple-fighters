using System.Collections.Generic;
using ComponentModel.Common;
using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharactersGetter : IExposableComponent
    {
        IEnumerable<CharacterParameters> GetCharacters(int userId);
    }
}