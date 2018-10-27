using System.Collections.Generic;

namespace Game.InterestManagement
{
    public class Matrix2Comparer : IEqualityComparer<Matrix2>
    {
        public bool Equals(Matrix2 x, Matrix2 y)
        {
            return x.Row == y.Row && x.Column == y.Column;
        }

        public int GetHashCode(Matrix2 matrix2)
        {
            return matrix2.GetHashCode();
        }
    }
}