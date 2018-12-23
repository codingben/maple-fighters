using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class Entity : MonoBehaviour, ISceneObject
    {
        public int Id { get; set; }

        public GameObject GameObject => gameObject;

        private static int id;
        private IScene scene;

        private void Awake()
        {
            Id = ++id;
            name = $"{name} (Id: {Id})";
        }

        private void Start()
        {
            scene = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG).GetComponent<IScene>();
            scene?.AddSceneObject(this);
        }

        private void OnDestroy()
        {
            scene?.RemoveSceneObject(this);
        }
    }
}