using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class Map2DummyEntitiesProvider : DummySceneObjectsProviderBase
    {
        protected override IEnumerable<DummySceneObject> GetDummySceneObjects()
        {
            yield return new DummySceneObject
            {
                Id = 1,
                Name = "BlueSnail",
                Position = new Vector2(-2, -8.1f),
                SpawnDirection = Directions.Left,
            };

            yield return new DummySceneObject
            {
                Id = 2,
                Name = "Portal",
                Position = new Vector2(12.5f, -1.125f),
                SpawnDirection = Directions.Left,
                AddComponentsAction = (gameObject) =>
                {
                    var portalController =
                        gameObject.AddComponent<DummyPortalController>();
                    portalController.CreateTeleportation(Maps.Map_1);
                }
            };
        }
    }
}