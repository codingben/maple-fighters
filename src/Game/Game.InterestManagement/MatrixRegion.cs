using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<IRegion> GetRegions(IEnumerable<Vector2> positions)
        {
            var matrices = GetMatrices(positions);

            foreach (var matrix in matrices)
            {
                yield return regions[matrix.Row, matrix.Column];
            }
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }

        private IEnumerable<Matrix2> GetMatrices(IEnumerable<Vector2> positions)
        {
            var matrices = new List<Matrix2>();

            foreach (var position in positions)
            {
                var x = Math.Abs(position.X - (-(sceneSize.X / 2)));
                var y = Math.Abs(position.Y - (-(sceneSize.Y / 2)));

                var row = (int)Math.Floor(x / regionSize.X);
                var column = (int)Math.Floor(y / regionSize.Y);

                // Make sure it is not out of bounds.
                if ((row < 0 || row >= rows)
                    || (column < 0 || column >= columns))
                {
                    continue;
                }

                matrices.Add(new Matrix2(row, column));
            }

            return matrices.Distinct(new Matrix2Comparer());
        }

        public void Dispose()
        {
            foreach (var region in regions)
            {
                region?.Dispose();
            }
        }
    }
}