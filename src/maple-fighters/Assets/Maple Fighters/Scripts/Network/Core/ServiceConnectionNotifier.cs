using System;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    internal class ServiceConnectionNotifier : IServiceConnectionNotifier
    {
        public event Action Connected;

        public event Action<DisconnectReason, string> Disconnected;

        public void Connection()
        {
            Connected?.Invoke();
        }

        public void Disconnection(DisconnectReason reason, string details)
        {
            Disconnected?.Invoke(reason, details);
        }
    }
}