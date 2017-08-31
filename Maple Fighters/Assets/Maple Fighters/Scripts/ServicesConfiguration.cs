using Scripts.Utils;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServicesConfiguration", menuName = "Maple Fighters/Scriptable Objects/ServicesConfiguration", order = 2)]
    public class ServicesConfiguration : ScriptableObjectSingleton<ServicesConfiguration>
    {
        public ConnectionInformation GameConnectionInformation => gameConnectionInformation;

        [SerializeField] private ConnectionInformation gameConnectionInformation;
    }
}