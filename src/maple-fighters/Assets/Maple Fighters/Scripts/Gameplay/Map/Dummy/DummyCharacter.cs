using System;
using Scripts.Gameplay.Player;

namespace Scripts.Gameplay.Map.Dummy
{
    [Serializable]
    public class DummyCharacter
    {
        public DummyEntity DummyEntity;

        public string CharacterName = "Dummy";

        public CharacterClasses CharacterClass;
    }
}