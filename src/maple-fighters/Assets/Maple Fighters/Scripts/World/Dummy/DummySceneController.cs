using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummySceneController : MonoBehaviour
    {
        #if UNITY_EDITOR
        private const string DummySceneTag = "Scene";

        private void Awake()
        {
            var gameConfiguration = GameConfiguration.GetInstance();
            if (gameConfiguration != null)
            {
                if (gameConfiguration.Environment != Environment.Development)
                {
                    var dummyScene =
                        GameObject.FindGameObjectWithTag(DummySceneTag);
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