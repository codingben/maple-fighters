using UnityEngine;
using GameObject = UnityEngine.GameObject;

namespace Scripts.Gameplay.Actors.Entity
{
    public class Entity : MonoBehaviour, IEntity
    {
        public int Id { get; set; }
        public GameObject GameObject { get; private set; }

        private void Awake()
        {
            GameObject = gameObject;
        }
    }
}