namespace Game.Physics
{
    public static class PhysicsSettings
    {
        public static float TimeStep { get; set; } = 1.0f / 25.0f; // (25Hz)

        public static int VelocityIterations { get; set; } = 8;

        public static int PositionIterations { get; set; } = 3;

        public static float UpdatesPerSecond { get; set; } = 30.0f;

        public static float FramesPerSecond { get; set; } = 30.0f;

        public static float MaxTravelDistance { get; set; } = 1.0f;
    }
}