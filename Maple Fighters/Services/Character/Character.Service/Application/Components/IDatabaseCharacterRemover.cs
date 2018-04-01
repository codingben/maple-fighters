namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterRemover
    {
        void Remove(int userId, int characterIndex);
    }
}