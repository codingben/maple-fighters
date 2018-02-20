using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterRemover : IExposableComponent
    {
        bool Remove(int userId, int characterIndex);
    }
}