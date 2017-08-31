using CommonTools.Log;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    public class ScriptableObjectSingleton<T> : ScriptableObject
        where T : ScriptableObject
    {
        private const string NETWORK_CONFIGURATION_PATH = "Configurations/{0}";

        public static T GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = Resources.Load<T>(string.Format(NETWORK_CONFIGURATION_PATH, typeof(T).Name));

            if (_instance == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find {typeof(T).Name}"));
            }

            return _instance;
        }

        private static T _instance;
    }
}