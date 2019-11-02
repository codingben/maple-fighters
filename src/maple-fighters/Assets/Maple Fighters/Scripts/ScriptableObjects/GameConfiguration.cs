using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "GameConfiguration",
        menuName = "Scriptable Objects/GameConfiguration",
        order = 3)]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public HostingEnvironment Environment;

        public bool IsProduction()
        {
            return Environment == HostingEnvironment.Production;
        }

        public bool IsDevelopment()
        {
            return Environment == HostingEnvironment.Development;
        }
    }
}