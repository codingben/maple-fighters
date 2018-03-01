using System.Linq;
using Scripts.Services;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServicesConfiguration", menuName = "Scriptable Objects/ServicesConfiguration", order = 1)]
    public class ServicesConfiguration : ScriptableObjectSingleton<ServicesConfiguration>
    {
        public ConnectionInformation GetConnectionInformation(ServerType type) 
            => connectionInformations.FirstOrDefault(connectionInformation => connectionInformation.ServerType == type);

        [SerializeField] private ConnectionInformation[] connectionInformations;
    }
}