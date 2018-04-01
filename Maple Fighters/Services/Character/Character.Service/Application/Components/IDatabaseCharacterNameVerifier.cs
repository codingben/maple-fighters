namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterNameVerifier
    {
        bool Verify(string name);
    }
}