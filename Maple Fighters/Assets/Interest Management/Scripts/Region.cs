using System.Collections.Generic;
using UnityEngine;

namespace InterestManagement
{
    public class Region : MonoBehaviour
    {
        public Rectangle Rectangle { get; set; }

        private readonly Dictionary<int, GameObject> entities = new Dictionary<int, GameObject>();

        private void Update()
        {
            print($"Region: {name} Entities: {entities.Count}");
        }

        public void AddEntity(int id)
        {
            if (entities.ContainsKey(id))
            {
                Debug.LogError($"An entity with id {id} already exists");
                return;
            }

            entities.Add(id, gameObject);
        }

        public void RemoveEntity(int id)
        {
            if (!entities.ContainsKey(id))
            {
                Debug.LogError($"An entity with id {id} does not exists");
                return;
            }

            entities.Remove(id);
        }

        public bool GetEntity(int id)
        {
            return entities.ContainsKey(id);
        }
    }
}