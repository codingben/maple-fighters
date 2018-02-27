using Character.Client.Common;
using ComponentModel.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterExistence : IExposableComponent
    {
        bool Exists(int userId, CharacterIndex characterIndex);
    }
}