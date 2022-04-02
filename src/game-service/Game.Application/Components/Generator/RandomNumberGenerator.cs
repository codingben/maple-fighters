using System;

namespace Game.Application.Components
{
    public class RandomNumberGenerator : ComponentBase, IRandomNumberGenerator
    {
        private readonly Random random = new();

        public int GenerateRandomNumber()
        {
            return random.Next(int.MinValue, int.MaxValue);
        }

        public int GenerateRandomNumber(int minimum, int maximum)
        {
            return random.Next(minimum, maximum);
        }
    }
}