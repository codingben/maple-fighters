using System;
using CommonTools.Coroutines;

namespace ServerCommon.Application
{
    public class TimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            var time = (float)(Environment.TickCount * 1E-03);
            return time;
        }
    }
}