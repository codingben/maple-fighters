using System;
using System.Windows.Forms;
using CommonTools.Log;

namespace PhotonControl
{
    internal class Logger : ILogger
    {
        public void Log(string message, LogMessageType type = LogMessageType.Log, object context = null)
        {
            switch (type)
            {
                case LogMessageType.Log:
                case LogMessageType.Warning:
                case LogMessageType.Error:
                {
                    MessageBox.Show(message, type.ToString());
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        public void Break()
        {
            // Left blank intentionally
        }
    }
}