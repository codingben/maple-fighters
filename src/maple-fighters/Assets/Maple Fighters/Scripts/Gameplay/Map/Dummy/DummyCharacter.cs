using System;
using Game.Common;

namespace Scripts.Gameplay.Map.Dummy
{
    [Serializable]
    public class DummyCharacter
    {
        public DummyEntity DummyEntity;
        public CharacterClasses CharacterClass;
        public CharacterIndex CharacterIndex = CharacterIndex.Zero;
    }
}