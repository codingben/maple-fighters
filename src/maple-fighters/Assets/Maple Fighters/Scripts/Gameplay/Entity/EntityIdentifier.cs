using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    [DisallowMultipleComponent]
    public class EntityIdentifier : MonoBehaviour, IEntity
    {
        public int Id
        {
            get => id;

            set => id = value;
        }

        public GameObject GameObject => gameObject;

        [ViewOnly, SerializeField]
        private int id;
    }
}