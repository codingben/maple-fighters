using System;
using CommonTools.Coroutines;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public class FiberCoroutinesExecutor : ThreadSafeCoroutinesExecutorBase
    {
        private readonly IDisposable scheduler;

        public FiberCoroutinesExecutor(IScheduler fiber, int updateRateMilliseconds)
        {
            scheduler = 
                fiber.ScheduleOnInterval(Update, 0, updateRateMilliseconds);
        }

        public override void Dispose()
        {
            scheduler.Dispose();

            base.Dispose();
        }
    }
}