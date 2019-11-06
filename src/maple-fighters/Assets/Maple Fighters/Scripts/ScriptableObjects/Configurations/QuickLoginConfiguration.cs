using ScriptableObjects.Utils;
using UnityEngine;

namespace ScriptableObjects.Configurations
{
    [CreateAssetMenu(
        fileName = "QuickLoginConfiguration",
        menuName = "Configurations/QuickLoginConfiguration",
        order = 1)]
    public class QuickLoginConfiguration : ScriptableSingleton<QuickLoginConfiguration>
    {
        public string Email;
        public string Password;
    }
}