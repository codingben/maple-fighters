using System;
using Common.ComponentModel;

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
                throw new ServerApplicationException(
                    "The server components provider has already been initialized.");
            }

            _components =
                components ?? throw new ArgumentNullException(
                    nameof(components));
        }

        public static IExposedComponents Provide()
        {
            if (_components == null)
            {
                throw new ServerApplicationException(
                    "The server components provider is not initialized.");
            }

            return _components;
        }
    }
}