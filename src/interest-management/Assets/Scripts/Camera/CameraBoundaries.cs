using UnityEngine;

namespace Game.InterestManagement.Simulation.Camera
{
    public struct CameraBoundaries
    {
        private readonly Vector2 upperBound;
        private readonly Vector2 lowerBound;

        public CameraBoundaries(Vector2 upperBound, Vector2 lowerBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }
        
        public Vector3 Transform(Vector3 position)
        {
            var x = Mathf.Clamp(position.x, lowerBound.x, upperBound.x);
            var y = Mathf.Clamp(position.y, lowerBound.y, upperBound.y);

            return new Vector3(x, y, position.z);
        }
    }
}