using System;
using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ServerShutdownTracker : ComponentBase, IServerShutdownNotifier
    {
        public event Action Shutdown;

        protected override void OnRemoved()
        {
            base.OnRemoved();

            Shutdown?.Invoke();
        }
    }
}