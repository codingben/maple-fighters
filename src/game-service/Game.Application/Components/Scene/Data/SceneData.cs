namespace Game.Application.Components
{
    public class SceneData
    {
        public ObjectData[] Objects { get; set; }

        public Vector2Data SceneSize { get; set; }

        public Vector2Data RegionSize { get; set; }

        public ScenePlayerSpawnData PlayerSpawn { get; set; }
    }
}