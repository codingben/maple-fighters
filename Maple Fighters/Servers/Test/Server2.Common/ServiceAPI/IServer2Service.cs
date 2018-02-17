using System;
using CommonCommunicationInterfaces;
using ServerApplication.Common.Components;

namespace Server2.Common
{
    public interface IServer2Service : IServiceBase
    {
        event Action<EmptyParameters> TestAction;
    }
}