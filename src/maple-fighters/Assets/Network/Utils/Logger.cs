using System;
using CommonTools.Log;
using UnityEngine;

namespace Network.Utils
{
    public class Logger : CommonTools.Log.ILogger
    {
        public void Log(string message, LogMessageType type = LogMessageType.Log, object context = null)
        {
            switch (type)
            {
                case LogMessageType.Log:
                {
                    Debug.Log(message);
                    break;
                }

                case LogMessageType.Warning:
                {
                    Debug.LogWarning(message);
                    break;
                }

                case LogMessageType.Error:
                {
                    Debug.LogError(message);
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
            Debug.Break();
        }
    }
}