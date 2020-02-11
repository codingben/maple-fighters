using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class MatrixRegion<TObject> : IMatrixRegion<TObject>
        where TObject : ISceneObject
    {
        private readonly Vector2 sceneSize;
        private readonly Vector2 regionSize;
        private readonly int rows;
        private readonly int columns;
        private readonly IRegion<TObject>[,] regions;
        private readonly object locker = new object();

        // TODO: Optimize
        private HashSet<IRegion<TObject>> temporaryRegions;
        private SceneBoundaries sceneBoundaries;

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
                        regions = new IRegion<TObject>[rows, columns];
                    }

                    var x2 = x1 + (row * regionSize.X);
                    var y2 = y1 + (column * regionSize.Y);
                    var position = new Vector2(x2, y2);

                    regions[row, column] =
                        new Region<TObject>(position, regionSize);
                }
            }

            sceneBoundaries = new SceneBoundaries(
                upperBound: new Vector2(sceneSize.X / 2, sceneSize.Y / 2), 
                lowerBound: new Vector2(sceneSize.X / 2, sceneSize.Y / 2) * -1);
        }

        public void Dispose()
        {
            foreach (var region in regions)
            {
                region?.Dispose();
            }
        }

        public IEnumerable<IRegion<TObject>> GetRegions(
            IEnumerable<Vector2> points)
        {
            lock (locker)
            {
                if (temporaryRegions == null)
                {
                    temporaryRegions = new HashSet<IRegion<TObject>>();
                }
                else
                {
                    temporaryRegions.Clear();
                }

                foreach (var point in points)
                {
                    if (sceneBoundaries.WithinBoundaries(point))
                    {
                        var row = (int)Math.Floor(
                            Math.Abs(point.X - (-(sceneSize.X / 2)))
                            / regionSize.X);
                        var column = (int)Math.Floor(
                            Math.Abs(point.Y - (-(sceneSize.Y / 2)))
                            / regionSize.Y);

                        if (row >= rows || column >= columns)
                        {
                            continue;
                        }

                        var region = regions[row, column];
                        temporaryRegions.Add(region);
                    }
                }

                return temporaryRegions;
            }
        }

        public IRegion<TObject>[,] GetAllRegions()
        {
            return regions;
        }
    }
}