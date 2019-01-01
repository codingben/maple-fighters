using System.Linq;
using Scripts.Services;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "ServicesConfiguration",
        menuName = "Scriptable Objects/ServicesConfiguration",
        order = 1)]
    public class ServicesConfiguration : ScriptableSingleton<ServicesConfiguration>
    {
        [SerializeField]
        private ConnectionInformation[] connectionInformations;

        public ConnectionInformation GetConnectionInformation(
            ServerType serverType)
        {
            return connectionInformations.FirstOrDefault(
                x => x.ServerType == serverType);
        }
    }
}