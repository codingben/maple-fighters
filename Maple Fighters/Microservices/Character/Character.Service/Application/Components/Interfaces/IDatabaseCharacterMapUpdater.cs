namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharacterMapUpdater
    {
        void Update(int userId, byte map);
    }
}