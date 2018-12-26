using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class NetworkIdentity : MonoBehaviour, ISceneObject
    {
        [ViewOnly, SerializeField] private int id;

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

        public GameObject GameObject => gameObject;
    }
}