namespace Game.Application.Components
{
    public class IdGenerator : ComponentBase, IIdGenerator
    {
        private readonly object locker = new();
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