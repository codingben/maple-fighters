using System;
using CommonTools.Log;
using UnityEngine;

namespace Scripts.Services
{
    using ILogger = CommonTools.Log.ILogger;

    public class Logger : ILogger
    {
        public void Log(
            string message,
            LogMessageType type = LogMessageType.Log,
            object context = null)
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