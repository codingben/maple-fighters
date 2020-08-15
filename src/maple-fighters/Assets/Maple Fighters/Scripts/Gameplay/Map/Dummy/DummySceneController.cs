using ScriptableObjects.Configurations;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    public class DummySceneController : MonoBehaviour
    {
        #if UNITY_EDITOR
        private void Awake()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                if (networkConfiguration.IsProduction())
                {
                    var dummyScene =
                        GameObject.FindGameObjectWithTag(GameTags.DummySceneTag);
                    if (dummyScene != null)
                    {
                        Destroy(dummyScene);
                    }
                }
            }

            Destroy(gameObject);
        }
        #endif
    }
}