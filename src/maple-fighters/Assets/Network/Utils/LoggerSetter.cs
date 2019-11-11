using CommonTools.Log;
using UnityEngine;

namespace Network.Utils
{
    public class LoggerSetter : MonoBehaviour
    {
        private void Awake()
        {
            if (LogUtils.Logger == null)
            {
                LogUtils.Logger = new Logger();
            }

            Destroy(gameObject);
        }
    }
}