using System;
using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;

namespace ServerApplication.Common.Components
{
    public class RandomNumberGenerator : Component<IServerEntity>, IRandomNumberGenerator
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

        public int GenerateRandomNumber(int min, int max)
        {
            lock (locker)
            {
                return random.Next(min, max);
            }
        }
    }
}