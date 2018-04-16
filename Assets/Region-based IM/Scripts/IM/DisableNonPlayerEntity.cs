using UnityEngine;

namespace InterestManagement.Scripts
{
    public class DisableNonPlayerEntity : MonoBehaviour
    {
        private ISceneEvents sceneEvents;

        private void Awake()
        {
            sceneEvents = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG).GetComponent<ISceneEvents>();
            if (sceneEvents != null)
            {
                sceneEvents.RegionsCreated += OnRegionsCreated;
            }
        }

        private void OnDestroy()
        {
            if (sceneEvents != null)
            {
                sceneEvents.RegionsCreated -= OnRegionsCreated;
            }
        }

        private void OnRegionsCreated()
        {
            DisableGameObject();
        }

        private void DisableGameObject()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}