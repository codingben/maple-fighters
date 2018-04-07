using System;
using CommonTools.Coroutines;
using ServerCommunicationInterfaces;

namespace Components.Common
{
    public class FiberCoroutinesExecutor : ThreadSafeCoroutinesExecutorBase
    {
        private readonly IDisposable scheduler;

        public FiberCoroutinesExecutor(IScheduler fiber, int updateRateMs)
        {
            scheduler = fiber.ScheduleOnInterval(Update, 0, updateRateMs);
        }

        public override void Dispose()
        {
            scheduler.Dispose();

            base.Dispose();
        }
    }
}