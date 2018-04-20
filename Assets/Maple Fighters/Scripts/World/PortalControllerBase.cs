using System;
using CommonTools.Coroutines;
using Scripts.Coroutines;
using UnityEngine;

namespace Scripts.World
{
    public class PortalControllerBase : MonoBehaviour, IPortalControllerBase, IDisposable
    {
        protected readonly ExternalCoroutinesExecutor CoroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Awake()
        {
            CoroutinesExecutor.ExecuteExternally();
        }

        public void Dispose()
        {
            CoroutinesExecutor.RemoveFromExternalExecutor();
        }

        public virtual void StartInteraction()
        {
            // Left blank intentionally
        }

        public virtual void StopInteraction()
        {
            // Left blank intentionally
        }
    }
}