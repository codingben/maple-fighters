using CommonTools.Log;
using Scripts.Gameplay.Actors.Entity;
using UnityEngine;

namespace Scripts.Utils
{
    public class MonoBehaviourEntity : MonoBehaviour
    {
        protected IEntity Entity { get; private set; }

        private void Awake()
        {
            Entity = transform.root.GetComponent<IEntity>().AssertNotNull();
        }
    }
}