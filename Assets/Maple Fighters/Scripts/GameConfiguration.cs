using UnityEngine;

namespace Scripts.ScriptableObjects
{
    public enum Environment
    {
        /// <summary>
        /// The production.
        /// </summary>
        Production,

        /// <summary>
        /// The development.
        /// </summary>
        Development
    }

    [CreateAssetMenu(
        fileName = "GameConfiguration",
        menuName = "Scriptable Objects/GameConfiguration",
        order = 3)]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public Environment Environment;
    }
}