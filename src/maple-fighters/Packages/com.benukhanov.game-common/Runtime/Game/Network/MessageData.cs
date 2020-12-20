namespace Game.Network
{
    public class MessageData
    {
        public byte Code { get; set; }

        public byte[] RawData { get; set; } // Newtonsoft.Json.Bson
    }
}