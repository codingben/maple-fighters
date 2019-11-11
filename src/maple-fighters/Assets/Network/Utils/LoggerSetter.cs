using System;
using CommonTools.Log;
using UnityEngine;

namespace Network.Utils
{
    public class LoggerSetter : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                if (LogUtils.Logger == null)
                {
                    LogUtils.Logger = new Logger();
                }
            }
            catch (Exception)
            {
                // Left blank intentionally
            }

            Destroy(gameObject);
        }
    }
}