using ComponentModel.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterNameVerifier : IExposableComponent
    {
        bool Verify(string name);
    }
}