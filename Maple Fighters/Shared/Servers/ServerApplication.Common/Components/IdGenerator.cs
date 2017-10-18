using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;

namespace ServerApplication.Common.Components
{
    public class IdGenerator : Component<IServerEntity>, IIdGenerator
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