using System;
using CommonTools.Coroutines;

namespace ServerCommon.TimeProviders
{
    public class EnvTimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            return (float)(Environment.TickCount * 1E-03);
        }
    }
}