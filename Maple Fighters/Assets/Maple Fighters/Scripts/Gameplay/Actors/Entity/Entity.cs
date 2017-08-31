using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors.Entity
{
    public class Entity : MonoBehaviour, IEntity
    {
        public int Id { get; set; }
        public EntityType Type { get; set; }
        public GameObject GameObject { get; private set; }

        private void Awake()
        {
            GameObject = gameObject;
        }
    }
}