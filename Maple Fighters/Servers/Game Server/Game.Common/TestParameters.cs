using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct TestParameters : IParameters
    {
        private int number;

        public TestParameters(int number)
        {
            this.number = number;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(number);
        }

        public void Deserialize(BinaryReader reader)
        {
            number = reader.ReadInt32();
        }
    }
}