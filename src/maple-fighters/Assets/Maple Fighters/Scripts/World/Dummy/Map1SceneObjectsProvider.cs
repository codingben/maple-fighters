using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class Map1SceneObjectsProvider : DummySceneObjectsProviderBase
    {
        protected override IEnumerable<DummySceneObject> GetDummySceneObjects()
        {
            yield return new DummySceneObject
            {
                Id = 1,
                Name = "Guardian",
                Position = new Vector2(-14.24f, -1.95f),
                SpawnDirection = Directions.Right
            };

            yield return new DummySceneObject
            {
                Id = 2,
                Name = "Portal",
                Position = new Vector2(-17.125f, -1.5f),
                SpawnDirection = Directions.Left,
                AddComponentsAction = (gameObject) => 
                {
                    var portalController = gameObject.AddComponent<DummyPortalController>();
                    portalController.CreateTeleportation(Maps.Map_2);
                }
            };
        }
    }
}