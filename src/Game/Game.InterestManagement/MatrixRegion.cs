using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class MatrixRegion : IMatrixRegion
    {
        private readonly Vector2 worldSize;
        private readonly Vector2 regionSize;
        private readonly int rows;
        private readonly int columns;
        private readonly IRegion[,] regions;
        private Bounds worldBounds;

        public MatrixRegion(Vector2 worldSize, Vector2 regionSize)
        {
            this.worldSize = worldSize;
            this.regionSize = regionSize;

            rows = (int)(worldSize.X / regionSize.X);
            columns = (int)(worldSize.Y / regionSize.Y);

            var x1 = -(worldSize.X / 2) + (regionSize.X / 2);
            var y1 = -(worldSize.Y / 2) + (regionSize.Y / 2);

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    if (regions == null)
                    {
                        regions = new IRegion[rows, columns];
                    }

                    var x2 = x1 + (row * regionSize.X);
                    var y2 = y1 + (column * regionSize.Y);
                    var position = new Vector2(x2, y2);

                    regions[row, column] = new Region(position, regionSize);
                }
            }

            var upperBound = new Vector2(worldSize.X / 2, worldSize.Y / 2);
            var lowerBound = new Vector2(worldSize.X / 2, worldSize.Y / 2) * -1;
            worldBounds = new Bounds(upperBound, lowerBound);
        }

        public void Dispose()
        {
            foreach (var region in regions)
            {
                region?.Dispose();
            }
        }

        public IEnumerable<IRegion> GetRegions(IEnumerable<Vector2> points)
        {
            foreach (var point in points)
            {
                if (worldBounds.IsInsideBounds(point))
                {
                    var row = (int)Math.Floor(
                        Math.Abs(point.X - (-(worldSize.X / 2)))
                        / regionSize.X);
                    var column = (int)Math.Floor(
                        Math.Abs(point.Y - (-(worldSize.Y / 2)))
                        / regionSize.Y);

                    if (row >= rows || column >= columns)
                    {
                        continue;
                    }

                    yield return regions[row, column];
                }
            }
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }
    }
}