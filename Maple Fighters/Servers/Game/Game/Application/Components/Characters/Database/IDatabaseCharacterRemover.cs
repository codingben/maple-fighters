using ComponentModel.Common;

namespace Game.Application.Components
{
    internal interface IDatabaseCharacterRemover : IExposableComponent
    {
        bool Remove(int userId, int characterIndex);
    }
}