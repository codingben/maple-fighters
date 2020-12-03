using System.IO;
using CommonCommunicationInterfaces;

namespace Chat.Common
{
    public struct ChatMessageEventParameters : IParameters
    {
        public string Message;

        public ChatMessageEventParameters(string message)
        {
            Message = message;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Message);
        }

        public void Deserialize(BinaryReader reader)
        {
            Message = reader.ReadString();
        }
    }
}