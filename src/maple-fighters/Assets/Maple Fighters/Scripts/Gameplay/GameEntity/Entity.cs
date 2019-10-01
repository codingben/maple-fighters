using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    public class Entity : MonoBehaviour, IEntity
    {
        [ViewOnly, SerializeField] private int id;

        public int Id
        {
            get => id;

            set => id = value;
        }

        public GameObject GameObject => gameObject;
    }
}