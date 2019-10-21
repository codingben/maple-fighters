using Scripts.Constants;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummySceneController : MonoBehaviour
    {
        #if UNITY_EDITOR
        private void Awake()
        {
            var gameConfiguration = GameConfiguration.GetInstance();
            if (gameConfiguration != null)
            {
                if (gameConfiguration.Environment != HostingEnvironment.Development)
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