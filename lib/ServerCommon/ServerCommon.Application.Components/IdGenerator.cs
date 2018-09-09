using Common.ComponentModel;

namespace ServerCommon.Application.Components
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