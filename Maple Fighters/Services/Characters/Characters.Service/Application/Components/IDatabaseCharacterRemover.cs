using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterRemover : IExposableComponent
    {
        void Remove(int userId, int characterIndex);
    }
}