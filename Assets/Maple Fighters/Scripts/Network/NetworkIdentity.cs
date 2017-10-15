using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class NetworkIdentity : MonoBehaviour, IGameObject
    {
        [ReadOnly, SerializeField] private int id;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}