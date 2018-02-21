using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterNameVerifier : IExposableComponent
    {
        bool Verify(string name);
    }
}