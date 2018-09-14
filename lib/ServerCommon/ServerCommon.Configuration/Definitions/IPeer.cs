namespace ServerCommon.Configuration.Definitions
{
    public interface IPeer
    {
        IPeerOperations Operations { get; set; }

        bool LogEvents { get; set; }
    }
}