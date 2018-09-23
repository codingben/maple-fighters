using System;
using Common.ComponentModel;

namespace ServerCommon.Application
{
    /// <summary>
    /// A global access across all the server to the exposed components.
    /// </summary>
    public static class ServerExposedComponents
    {
        private static IExposedComponentsProvider _components;

        public static void SetProvider(IExposedComponentsProvider components)
        {
            if (_components != null)
            {
                throw new ServerApplicationException(
                    "The server components provider has already been initialized.");
            }

            _components = components
                          ?? throw new ArgumentNullException(
                              nameof(components));
        }

        public static IExposedComponentsProvider Provide()
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