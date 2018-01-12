using System;

namespace Scripts.Services
{
    public class UnityEvent<T> : UnityEngine.Events.UnityEvent<T>
    {
        // Left blank intentionally
        internal void AddListener()
        {
            throw new NotImplementedException();
        }
    }
}