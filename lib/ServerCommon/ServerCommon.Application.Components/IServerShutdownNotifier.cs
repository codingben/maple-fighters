using System;

namespace ServerCommon.Application.Components
{
    public interface IServerShutdownNotifier
    {
        event Action Shutdown;
    }
}