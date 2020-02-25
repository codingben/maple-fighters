namespace ServerCommon.Configuration.Definitions
{
    public interface IDatabases
    {
        IMongo Mongo { get; set; }
    }
}