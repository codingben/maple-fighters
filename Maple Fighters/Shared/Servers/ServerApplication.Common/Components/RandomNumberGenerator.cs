using System;
using ComponentModel.Common;

namespace ServerApplication.Common.Components
{
    public class RandomNumberGenerator : Component, IRandomNumberGenerator
    {
        private readonly Random random = new Random();
        private readonly object locker = new object();

        public int GenerateRandomNumber()
        {
            lock (locker)
            {
                return random.Next(int.MinValue, int.MaxValue);
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