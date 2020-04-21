namespace Game.Physics
{
    public static class PhysicsSettings
    {
        public static float TimeStep = 1.0f / 25.0f; // (25Hz Simulation)
        public static int VelocityIterations = 8;
        public static int PositionIterations = 3;
        public static float UpdatesPerSecond = 30.0f;
        public static float FramesPerSecond = 30.0f;
        public static float MaxTravelDistance = 1.0f;
    }
}