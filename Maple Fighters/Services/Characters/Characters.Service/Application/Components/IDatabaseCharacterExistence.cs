using Characters.Client.Common;
using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterExistence : IExposableComponent
    {
        bool Exists(int userId, CharacterIndex characterIndex);
    }
}