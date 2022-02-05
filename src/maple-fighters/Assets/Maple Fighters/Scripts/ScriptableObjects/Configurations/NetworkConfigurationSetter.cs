using System.Runtime.InteropServices;
using UnityEngine;

namespace ScriptableObjects.Configurations
{
    public class NetworkConfigurationSetter : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void SetConfig();

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetConfig();
#endif
        }

        public void SetConfigCallback(string json)
        {
            var configuration = NetworkConfiguration.GetInstance();
            var data = JsonUtility.FromJson<NetworkConfigurationData>(json);
            var environment = data.Environment;

            configuration.HostingData = new HostingData[]
            {
                new HostingData()
                {
                    Host = data.DevHost ?? string.Empty,
                    Environment = HostingEnvironment.Development,
                },
                new HostingData()
                {
                    Host = data.Host ?? string.Empty,
                    Environment = HostingEnvironment.Production,
                }
            };
            configuration.Environment = ParseEnvironment(environment);
        }

        private HostingEnvironment ParseEnvironment(string environment)
        {
            if (environment == "Editor")
            {
                return HostingEnvironment.Editor;
            }
            else if (environment == "Production")
            {
                return HostingEnvironment.Production;
            }

            return HostingEnvironment.Development;
        }
    }
}