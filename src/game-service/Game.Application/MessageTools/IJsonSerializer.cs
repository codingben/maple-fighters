namespace Game.MessageTools
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data)
            where T : struct;

        T Deserialize<T>(string json)
            where T : struct;
    }
}