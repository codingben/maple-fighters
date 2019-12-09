using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    public static class Utils
    {
        public static GameObject CreateGameObject(string name, Vector3 position)
        {
            GameObject gameObject = null;

            var path = string.Format(Paths.Resources.Game.Entities, name);
            var entity = Resources.Load(path);
            if (entity != null)
            {
                gameObject =
                    Object.Instantiate(entity, position, Quaternion.identity)
                        as GameObject;

                if (gameObject != null)
                {
                    gameObject.name = name;
                }
            }
            else
            {
                Debug.LogError(
                    $"Could not find an object with name {name}");
            }

            return gameObject;
        }
    }
}