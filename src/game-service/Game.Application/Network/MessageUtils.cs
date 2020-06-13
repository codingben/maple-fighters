using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Game.Application.Network
{
    public static class MessageUtils
    {
        public static byte[] WrapMessage<T>(byte code, T message)
            where T : class
        {
            return SerializeMessage(new MessageData()
            {
                Code = code,
                RawData = SerializeMessage(message)
            });
        }

        public static T DeserializeMessage<T>(byte[] rawData)
            where T : class
        {
            T message;

            using (var reader = new BsonReader(new MemoryStream(rawData)))
            {
                var serializer = new JsonSerializer();
                message = serializer.Deserialize<T>(reader);
            }

            return message;
        }

        public static byte[] SerializeMessage<T>(T message)
            where T : class
        {
            var memoryStream = new MemoryStream();

            using (BsonWriter writer = new BsonWriter(memoryStream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, message);
            }

            return memoryStream.ToArray();
        }
    }
}