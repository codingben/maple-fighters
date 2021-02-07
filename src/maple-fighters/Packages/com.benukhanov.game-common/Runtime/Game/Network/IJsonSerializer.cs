namespace Game.Network
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data)
            where T : class;

        T Deserialize<T>(string json)
            where T : class;
    }
}