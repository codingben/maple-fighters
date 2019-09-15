using System.Linq;
using Scripts.Network;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "ServerConfiguration",
        menuName = "Scriptable Objects/ServerConfiguration",
        order = 1)]
    public class ServerConfiguration : ScriptableSingleton<ServerConfiguration>
    {
        public ConnectionInformation[] ConnectionInformations;

        public ConnectionInformation GetConnectionInformation(
            ServerType serverType)
        {
            return ConnectionInformations
                .FirstOrDefault(x => x.ServerType == serverType);
        }
    }
}