using UnityEngine;

namespace Scripts.Gameplay
{
    public class NetworkIdentity : MonoBehaviour, IGameObject
    {
        public int Id { get; set; }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}