using System.Collections.Generic;
using Character.Client.Common;
using ComponentModel.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharactersGetter : IExposableComponent
    {
        IEnumerable<CharacterFromDatabaseParameters> GetCharacters(int userId);
    }
}