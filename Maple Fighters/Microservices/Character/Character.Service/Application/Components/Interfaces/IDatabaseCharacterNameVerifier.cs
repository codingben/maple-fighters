namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharacterNameVerifier
    {
        bool Verify(string name);
    }
}