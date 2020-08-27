using InterestManagement;
using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    public partial class GameSceneVisualGraphics
    {
        public struct GameRegion
        {
            public int Index { get; }

            public IRegion<IGameObject> Region { get; }

            public Vector2 Position { get; }

            public Vector2 Size { get; }

            public GameRegion(int index, IRegion<IGameObject> region)
            {
                Index = index;
                Region = region;
                Position = ((Region<IGameObject>)region).GetPosition().FromVector2();
                Size = ((Region<IGameObject>)region).GetSize().FromVector2();
            }
        }
    }
}