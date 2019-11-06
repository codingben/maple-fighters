using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "QuickLoginConfiguration",
        menuName = "Scriptable Objects/QuickLoginConfiguration",
        order = 1)]
    public class QuickLoginConfiguration : ScriptableSingleton<QuickLoginConfiguration>
    {
        public string Email;
        public string Password;
    }
}