using Common.ComponentModel;

namespace Common.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class IdGenerator : ComponentBase, IIdGenerator
    {
        private uint id = uint.MinValue;

        public int GenerateId()
        {
            return (int)checked(++id);
        }
    }
}