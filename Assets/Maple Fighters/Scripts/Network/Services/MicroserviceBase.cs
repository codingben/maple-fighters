namespace Scripts.Services
{
    public class MicroserviceBase
    {
        protected IServerPeerHandler ServerPeerHandler;
        protected IServiceConnectionHandler ServiceConnectionHandler;

        protected MicroserviceBase(IServiceBase serviceBase)
        {
            ServerPeerHandler = serviceBase.ServerPeerHandler;
            ServiceConnectionHandler = serviceBase.ServiceConnectionHandler;
        }
    }
}