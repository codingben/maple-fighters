using System;

namespace Game.Messages
{
    [Serializable]
    public struct GameObjectData
    {
        public int Id;

        public string Name;

        public float X;

        public float Y;

        public float Direction;

        public string CharacterName;

        public byte CharacterClass;

        public bool HasCharacter;
    }
}