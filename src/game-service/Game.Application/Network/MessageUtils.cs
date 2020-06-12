using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Game.Application.Network
{
    public static class MessageUtils
    {
        public static T FromMessage<T>(byte[] rawData)
        {
            T message;

            using (var reader = new BsonReader(new MemoryStream(rawData)))
            {
                var serializer = new JsonSerializer();
                message = serializer.Deserialize<T>(reader);
            }

            return message;
        }

        public static byte[] ToMessage<T>(T message)
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