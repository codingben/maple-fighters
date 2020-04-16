using ScriptableObjects.Utils;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace ScriptableObjects.Configurations
{
    [CreateAssetMenu(
        fileName = "PlayerConfiguration",
        menuName = "Configurations/PlayerConfiguration",
        order = 2)]
    public class PlayerConfiguration : ScriptableSingleton<PlayerConfiguration>
    {
        public PlayerProperties PlayerProperties;
        public PlayerKeyboard PlayerKeyboard;
    }
}