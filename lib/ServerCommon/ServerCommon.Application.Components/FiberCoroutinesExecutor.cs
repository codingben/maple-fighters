using System;
using CommonTools.Coroutines;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public class FiberCoroutinesExecutor : ThreadSafeCoroutinesExecutorBase
    {
        private readonly IDisposable scheduler;

        public FiberCoroutinesExecutor(
            IScheduler fiber, int updateRateMilliseconds)
        {
            scheduler = fiber.ScheduleOnInterval(
                action: Update,
                firstInMs: 0,
                regularInMs: updateRateMilliseconds);
        }

        public override void Dispose()
        {
            scheduler.Dispose();

            base.Dispose();
        }
    }
}