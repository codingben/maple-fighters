using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.ApplicationBase
{
    public static class Server
    {
        public static IServerEntity Entity { get; } = new ServerEntity();
    }
}