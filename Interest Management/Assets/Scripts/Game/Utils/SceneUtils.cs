using UnityEngine;

namespace Assets.Scripts.Game.Utils
{
    public static class SceneUtils
    {
        public static GameScene GetSceneGameObject()
        {
            var scene = GameObject.FindGameObjectWithTag("Scene");
            if (scene == null)
            {
                Debug.LogError("Could not find the scene game object.");
                Debug.Break();
            }

            return scene?.GetComponent<GameScene>();
        }
    }
}