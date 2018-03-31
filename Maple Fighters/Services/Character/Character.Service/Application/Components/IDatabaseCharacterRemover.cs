using ComponentModel.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterRemover : IExposableComponent
    {
        void Remove(int userId, int characterIndex);
    }
}