namespace Game.Application.Messages
{
    public class EnteredSceneMessage
    {
        public int GameObjectId { get; set; }

        public SpawnPositionData SpawnPositionData { get; set; }
    }
}