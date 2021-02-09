namespace Game.Messages
{
    public struct EnteredSceneMessage
    {
        public int GameObjectId;

        public float X;

        public float Y;

        public float Direction;

        public string CharacterName;

        public byte CharacterClass;
    }
}