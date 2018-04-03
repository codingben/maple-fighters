using System.IO;
using CommonCommunicationInterfaces;

namespace Server2.Common
{
    public struct Server1OperationRequestParameters : IParameters
    {
        public int Number;

        public Server1OperationRequestParameters(int number)
        {
            Number = number;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Number);
        }

        public void Deserialize(BinaryReader reader)
        {
            Number = reader.ReadInt32();
        }
    }
}