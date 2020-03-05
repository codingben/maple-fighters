namespace ServerCommon.Configuration.Definitions
{
    public interface IPeerOperations
    {
        bool LogRequests { get; set; }

        bool LogResponses { get; set; }
    }
}