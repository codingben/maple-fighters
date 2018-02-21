using System.Collections.Generic;
using Characters.Client.Common;
using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharactersGetter : IExposableComponent
    {
        IEnumerable<CharacterFromDatabaseParameters> GetCharacters(int userId);
    }
}