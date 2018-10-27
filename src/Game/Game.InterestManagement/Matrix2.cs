namespace Game.InterestManagement
{
    public struct Matrix2
    {
        public readonly int Row;
        public readonly int Column;

        public Matrix2(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override int GetHashCode()
        {
            return Row ^ 2 + Column ^ 2;
        }
    }
}