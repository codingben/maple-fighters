﻿using InterestManagement;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Game.InterestManagement.Simulation
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField]
        private Vector2 sceneSize;

        [SerializeField]
        private Vector2 regionSize;

        private IScene<IGameObject> scene;

        private void Awake()
        {
            scene =
                new Scene<IGameObject>(sceneSize.ToVector2(), regionSize.ToVector2(), new UnityLogger());
        }

        public Vector2 GetSceneSize()
        {
            return sceneSize;
        }

        public IScene<IGameObject> GetScene()
        {
            return scene;
        }
    }
}