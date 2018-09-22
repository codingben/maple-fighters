using Common.ComponentModel;

namespace ServerCommon.Application
{
    public static class ServerExposedComponents
    {
        private static IExposedComponentsProvider _components;

        public static void SetProvider(IExposedComponentsProvider components)
        {
            _components = components;
        }

        public static IExposedComponentsProvider Provide()
        {
            if (_components == null)
            {
                throw new ServerApplicationException(
                    "Server components provider is not initialized.");
            }

            return _components;
        }
    }
}