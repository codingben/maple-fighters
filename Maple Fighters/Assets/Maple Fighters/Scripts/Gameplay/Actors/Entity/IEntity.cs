using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors.Entity
{
    public interface IEntity
    {
        int Id { get;}

        EntityType Type { get; }

        GameObject GameObject { get; }
    }
}