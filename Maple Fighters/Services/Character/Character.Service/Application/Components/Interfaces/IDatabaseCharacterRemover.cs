namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharacterRemover
    {
        void Remove(int userId, int characterIndex);
    }
}