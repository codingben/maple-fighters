using System;
using System.Collections.Generic;
using System.Linq;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class MatrixRegion : IMatrixRegion
    {
        public Vector2 SceneSize { get; }

        public Vector2 RegionSize { get; }

        private readonly int rows;
        private readonly int columns;
        private readonly IRegion[,] regions;

        public MatrixRegion(Vector2 sceneSize, Vector2 regionSize)
        {
            SceneSize = sceneSize;
            RegionSize = regionSize;

            rows = (int)(sceneSize.X / regionSize.X);
            columns = (int)(sceneSize.Y / regionSize.Y);
            regions = new IRegion[rows, columns];

            InitializeRegions();
        }

        private void InitializeRegions()
        {
            var x = -(SceneSize.X / 2) + (RegionSize.X / 2);
            var y = -(SceneSize.Y / 2) + (RegionSize.Y / 2);

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var position = new Vector2(
                        x: x + (row * RegionSize.X),
                        y: y + (column * RegionSize.Y));

                    regions[row, column] = new Region(position, RegionSize);
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

        private IEnumerable<Matrix2> GetMatrices(IEnumerable<Vector2> positions)
        {
            var matrices = new List<Matrix2>();

            foreach (var position in positions)
            {
                var x = Math.Abs(position.X - (-(SceneSize.X / 2)));
                var y = Math.Abs(position.Y - (-(SceneSize.Y / 2)));

                var row = (int)Math.Floor(x / RegionSize.X);
                var column = (int)Math.Floor(y / RegionSize.Y);

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