using System;
using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.Components
{
    public class RandomNumberGenerator : CommonComponent
    {
        private readonly Random random = new Random();
        private readonly object locker = new object();

        public int GenerateRandomNumber()
        {
            lock (locker)
            {
                return random.Next(0, int.MaxValue);
            }
        }
    }
}