namespace Physics.Box2D
{
    public static class DefaultSettings
    {
        public const float TimeStep = 1.0f / 25.0f; // (30Hz)

        public const int SleepTime = 10;

        public const int VelocityIterations = 8;

        public const int PositionIterations = 3;

        public const float UpdatesPerSecond = 30.0f;

        public const float FramesPerSecond = 30.0f;

        public const float MaxTravelDistance = 1.0f;
    }
}