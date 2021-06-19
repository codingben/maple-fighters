using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    public class CharacterData : ComponentBase, ICharacterData
    {
        public string Name { get; set; }

        public byte CharacterType { get; set; }

        public float SpawnDirection { get; set; }
    }
}