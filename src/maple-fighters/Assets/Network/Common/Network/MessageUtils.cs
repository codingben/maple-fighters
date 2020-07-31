using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Game.Network
{
    public static class MessageUtils
    {
        public static byte[] WrapMessage<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            return SerializeMessage(new MessageData()
            {
                Code = code,
                RawData = SerializeMessage(message)
            });
        }

        public static byte[] SerializeMessage<TMessage>(TMessage message)
            where TMessage : class
        {
            var memoryStream = new MemoryStream();

            using (BsonWriter writer = new BsonWriter(memoryStream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, message);
            }

            return memoryStream.ToArray();
        }

        public static TMessage DeserializeMessage<TMessage>(byte[] rawData)
            where TMessage : class
        {
            TMessage message;

            using (var reader = new BsonReader(new MemoryStream(rawData)))
            {
                var serializer = new JsonSerializer();
                message = serializer.Deserialize<TMessage>(reader);
            }

            return message;
        }
    }
}