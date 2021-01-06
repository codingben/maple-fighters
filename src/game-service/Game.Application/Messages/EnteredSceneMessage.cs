namespace Game.Messages
{
    public class EnteredSceneMessage
    {
        public int GameObjectId { get; set; }

        public SpawnData SpawnData { get; set; }

        public CharacterData CharacterData { get; set; }
    }
}