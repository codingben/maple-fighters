using CommonTools.Log;
using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;

namespace ServerApplication.Common.Components
{
    public class IdGenerator : Component, IIdGenerator
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

        public static int GetId()
        {
            var idGenerator = Server.Entity.GetComponent<IIdGenerator>().AssertNotNull();
            var id = idGenerator.GenerateId();
            return id;
        }
    }
}