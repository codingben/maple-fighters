using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class MatrixRegion : IMatrixRegion
    {
        private readonly Vector2 sceneSize;
        private readonly Vector2 regionSize;
        private readonly int rows;
        private readonly int columns;
        private readonly IRegion[,] regions;
        private HashSet<IRegion> cachedRegions;

        public MatrixRegion(Vector2 sceneSize, Vector2 regionSize)
        {
            this.sceneSize = sceneSize;
            this.regionSize = regionSize;

            rows = (int)(sceneSize.X / regionSize.X);
            columns = (int)(sceneSize.Y / regionSize.Y);

            var x1 = -(sceneSize.X / 2) + (regionSize.X / 2);
            var y1 = -(sceneSize.Y / 2) + (regionSize.Y / 2);

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
            if (cachedRegions == null)
            {
                cachedRegions = new HashSet<IRegion>();
            }
            else
            {
                cachedRegions.Clear();
            }

            foreach (var point in points)
            {
                var row = (int)Math.Floor(
                    Math.Abs(point.X - (-(sceneSize.X / 2))) / regionSize.X);
                var column = (int)Math.Floor(
                    Math.Abs(point.Y - (-(sceneSize.Y / 2))) / regionSize.Y);

                if ((row < 0 || row >= rows)
                    || (column < 0 || column >= columns))
                {
                    continue;
                }

                var region = regions[row, column];
                cachedRegions.Add(region);
            }

            return cachedRegions;
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }
    }
}