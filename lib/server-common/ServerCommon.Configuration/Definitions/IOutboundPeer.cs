namespace ServerCommon.Configuration.Definitions
{
    public interface IOutboundPeer
    {
        IPeerOperations Operations { get; set; }

        bool LogEvents { get; set; }
    }
}