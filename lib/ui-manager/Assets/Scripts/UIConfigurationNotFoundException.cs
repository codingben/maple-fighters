using System;

namespace UI
{
    public class UIConfigurationNotFoundException : Exception
    {
        public UIConfigurationNotFoundException()
            : base("UI configuration file was not found in the UI resources.")
        {
            // Left blank intentionally
        }
    }
}