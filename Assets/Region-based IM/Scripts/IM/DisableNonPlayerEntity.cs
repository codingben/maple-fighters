using UnityEngine;

namespace InterestManagement.Scripts
{
    public class DisableNonPlayerEntity : MonoBehaviour
    {
        private void Awake()
        {
            var sceneEvents = GameObject.FindGameObjectWithTag("Scene").GetComponent<ISceneEvents>();
            sceneEvents.RegionsCreated += OnRegionsCreated;
        }

        private void OnRegionsCreated()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}