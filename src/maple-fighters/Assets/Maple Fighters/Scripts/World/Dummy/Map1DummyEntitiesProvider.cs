using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class Map1DummyEntitiesProvider : MonoBehaviour, IDummyEntitiesProvider
    {
        public IEnumerable<DummyEntity> GetEntities()
        {
            yield return new DummyEntity
            {
                Id = 1,
                Name = "Guardian",
                Position = new Vector2(-14.24f, -1.95f),
                SpawnDirection = Directions.Right
            };

            yield return new DummyEntity
            {
                Id = 2,
                Name = "Portal",
                Position = new Vector2(-17.125f, -1.5f),
                SpawnDirection = Directions.Left,
            };
        }
    }
}