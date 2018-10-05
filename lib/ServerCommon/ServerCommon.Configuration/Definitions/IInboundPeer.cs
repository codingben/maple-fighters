namespace ServerCommon.Configuration.Definitions
{
    public interface IInboundPeer
    {
        IPeerOperations Operations { get; set; }

        bool LogEvents { get; set; }
    }
}