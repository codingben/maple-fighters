using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class NetworkIdentity : MonoBehaviour, ISceneObject
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
            return Equals(gameObject, null) ? null : gameObject;
        }
    }
}