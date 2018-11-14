using System;

namespace Game.InterestManagement
{
    public class InvalidMatrixRegionSize : Exception
    {
        public InvalidMatrixRegionSize()
            : base("The scene size or region size cannot be equal to zero.")
        {
            // Left blank intentionally
        }
    }
}