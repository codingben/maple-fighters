using System;

namespace ServerCommon.Configuration
{
    public class ConfigurationNotInitializedException : Exception
    {
        public ConfigurationNotInitializedException(string configName)
            : base($"The {configName} configuration is not initialized.")
        {
            // Left blank intentionally
        }
    }
}