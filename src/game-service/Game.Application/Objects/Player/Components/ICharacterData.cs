namespace Game.Application.Objects.Components
{
    public interface ICharacterData
    {
        string Name { get; set; }

        byte CharacterType { get; set; }

        float SpawnDirection { get; set; }
    }
}