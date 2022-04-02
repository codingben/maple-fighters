using Common.ComponentModel;

namespace Common.Components
{
    public class IdGenerator : ComponentBase, IIdGenerator
    {
        private readonly object locker = new object();
        private uint id = uint.MinValue;

        public int GenerateId()
        {
            lock (locker)
            {
                return (int)checked(++id);
            }
        }
    }
}