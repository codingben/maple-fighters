using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "QuickLoginConfiguration", menuName = "Scriptable Objects/QuickLoginConfiguration", order = 2)]
    public class QuickLoginConfiguration : ScriptableObjectSingleton<QuickLoginConfiguration>
    {
        public string Email;
        public string Password;
    }
}