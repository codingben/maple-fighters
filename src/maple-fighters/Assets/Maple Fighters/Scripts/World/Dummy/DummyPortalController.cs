using Game.Common;
using Scripts.Gameplay.Entity;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [RequireComponent(typeof(IEntity))]
    public class DummyPortalController : MonoBehaviour
    {
        public void CreateTeleportation(Maps map)
        {
            var entity = GetComponent<IEntity>();
            if (entity != null)
            {
                DummyPortalContainer.GetInstance().Add(entity.Id, map);
            }
        }
    }
}