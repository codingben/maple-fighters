using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.Components
{
    public class IdGenerator : Component
    {
        private uint id = uint.MinValue;
        private readonly object locker = new object();

        public int GenerateId()
        {
            lock (locker)
            {
                return (int) checked(id++);
            }
        }
    }
}