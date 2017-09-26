using System.Linq;
using Scripts.Services;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServicesConfiguration", menuName = "Maple Fighters/Scriptable Objects/ServicesConfiguration", order = 2)]
    public class ServicesConfiguration : ScriptableObjectSingleton<ServicesConfiguration>
    {
        public ConnectionInformation GetConnectionInformation(ServersType type) 
            => connectionInformations.FirstOrDefault(connectionInformation => connectionInformation.ServerType == type);

        [SerializeField] private ConnectionInformation[] connectionInformations;
    }
}