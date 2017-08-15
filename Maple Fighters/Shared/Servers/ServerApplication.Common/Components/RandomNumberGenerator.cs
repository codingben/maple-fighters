using System;
using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.Components
{
    public class RandomNumberGenerator : IComponent
    {
        private readonly Random random = new Random();
        private readonly object locker = new object();

        public int GenerateId()
        {
            lock (locker)
            {
                return random.Next(0, int.MaxValue);
            }
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}