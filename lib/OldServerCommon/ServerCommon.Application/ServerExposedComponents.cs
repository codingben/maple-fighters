using System;
using Common.ComponentModel;
using ServerCommon.Application.Exceptions;

namespace ServerCommon.Application
{
    /// <summary>
    /// A global access across all the server to the exposed components.
    /// </summary>
    public static class ServerExposedComponents
    {
        private static IExposedComponents _components;

        public static void SetProvider(IExposedComponents components)
        {
            if (_components != null)
            {
                throw new ServerComponentsAlreadyInitializedException();
            }

            _components = 
                components ?? throw new ArgumentNullException(nameof(components));
        }

        public static IExposedComponents Provide()
        {
            if (_components == null)
            {
                throw new ServerComponentsNotInitializedException();
            }

            return _components;
        }
    }
}