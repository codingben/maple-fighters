namespace Scripts.Services
{
    public interface IServiceBase : IPeerLogicBaseHandler
    {
        IServiceConnectionHandler ServiceConnectionHandler { get; }
    }
}