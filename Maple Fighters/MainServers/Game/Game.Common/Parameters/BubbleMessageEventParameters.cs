using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct BubbleMessageEventParameters : IParameters
    {
        public int RequesterId;
        public string Message;
        public int Time;

        public BubbleMessageEventParameters(int requesterId, string message, int time)
        {
            RequesterId = requesterId;
            Message = message;
            Time = time;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RequesterId);
            writer.Write(Message);
            writer.Write(Time);
        }

        public void Deserialize(BinaryReader reader)
        {
            RequesterId = reader.ReadInt32();
            Message = reader.ReadString();
            Time = reader.ReadInt32();
        }
    }
}