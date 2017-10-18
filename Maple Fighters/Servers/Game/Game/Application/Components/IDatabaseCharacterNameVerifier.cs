using ComponentModel.Common;

namespace Game.Application.Components
{
    internal interface IDatabaseCharacterNameVerifier : IExposableComponent
    {
        bool Verify(string name);
    }
}