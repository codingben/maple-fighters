namespace Scripts.Services
{
    public interface IServiceBase
    {
        IServiceConnectionHandler ServiceConnectionHandler { get; }
    }
}