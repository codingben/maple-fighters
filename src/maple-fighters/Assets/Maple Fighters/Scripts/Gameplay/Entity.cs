using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class Entity : MonoBehaviour, ISceneObject
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