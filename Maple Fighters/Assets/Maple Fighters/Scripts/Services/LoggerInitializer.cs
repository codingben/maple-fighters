using CommonTools.Log;
using UnityEngine;

namespace Scripts.Services
{
    public class LoggerInitializer : MonoBehaviour
    {
        private void Awake()
        {
            LogUtils.Logger = new Logger();
        }
    }
}