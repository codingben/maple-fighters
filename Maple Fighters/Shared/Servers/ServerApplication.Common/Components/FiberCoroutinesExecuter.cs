using System;
using ServerCommunicationInterfaces;
using CommonTools.Coroutines;

namespace ServerApplication.Common.Components.Coroutines
{
    internal class FiberCoroutinesExecuter : ThreadSafeCoroutinesExecuterBase
    {
        private readonly IDisposable scheduler;

        public FiberCoroutinesExecuter(IScheduler fiber, int updateRateMs)
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