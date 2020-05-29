using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Game.Application
{
    public static class MessageUtils
    {
        public static T GetMessage<T>(byte[] rawData)
        {
            T message;

            using (var reader = new BsonReader(new MemoryStream(rawData)))
            {
                var serializer = new JsonSerializer();
                message = serializer.Deserialize<T>(reader);
            }

            return message;
        }
    }
}