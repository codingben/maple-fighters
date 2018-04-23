using System.Collections.Generic;
using Game.Common;
using Scripts.World;
using UnityEngine;

namespace Assets.Scripts
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
                SpawnDirection = Directions.Right,

                AddComponents = AddCommonComponents
            };

            yield return new DummySceneObject
            {
                Id = 2,
                Name = "Portal",
                Position = new Vector2(-17.125f, -1.5f),
                SpawnDirection = Directions.Left,

                AddComponents = (gameObject) => 
                {
                    AddCommonComponents(gameObject);

                    var portalController = gameObject.AddComponent<DummyPortalController>();
                    portalController.CreateTeleportation(Maps.Map_2);
                }
            };
        }
    }
}