using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    public static class SceneUtils
    {
        public static GameScene GetSceneGameObject()
        {
            var gameScene =
                Object.FindObjectOfType(typeof(GameScene)) as GameScene;
            if (gameScene == null)
            {
                Debug.LogError("Could not find the game scene object.");
                Debug.Break();
            }

            return gameScene;
        }
    }
}