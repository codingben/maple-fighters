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

            configuration.ServerData = new ServerData[]
            {
                new ServerData()
                {
                    ServerType = ServerType.Authenticator,
                    Url = data.AuthUrl ?? string.Empty
                },
                new ServerData()
                {
                    ServerType = ServerType.GameProvider,
                    Url = data.GameProviderUrl ?? string.Empty
                },
                new ServerData()
                {
                    ServerType = ServerType.Character,
                    Url = data.CharacterUrl ?? string.Empty
                },
                new ServerData()
                {
                    ServerType = ServerType.Chat,
                    Url = data.ChatUrl ?? string.Empty
                }
            };
            configuration.HostingEnvironment = ParseEnvironment(data.Environment);
        }

        private HostingEnvironment ParseEnvironment(string environment)
        {
            if (environment == "Production")
            {
                return HostingEnvironment.Production;
            }

            return HostingEnvironment.Development;
        }
    }
}