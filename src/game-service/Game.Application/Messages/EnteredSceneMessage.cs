namespace Game.Messages
{
    public class EnteredSceneMessage
    {
        public int GameObjectId { get; set; }

        public SpawnPositionData SpawnPositionData { get; set; }

        public CharacterData CharacterData { get; set; }
    }
}