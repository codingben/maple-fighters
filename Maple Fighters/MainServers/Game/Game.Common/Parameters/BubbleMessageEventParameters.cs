using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct BubbleMessageEventParameters : IParameters
    {
        public int RequesterId;
        public string Message;

        public BubbleMessageEventParameters(int requesterId, string message)
        {
            RequesterId = requesterId;
            Message = message;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RequesterId);
            writer.Write(Message);
        }

        public void Deserialize(BinaryReader reader)
        {
            RequesterId = reader.ReadInt32();
            Message = reader.ReadString();
        }
    }
}