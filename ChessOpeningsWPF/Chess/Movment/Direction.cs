namespace ChessOpeningsWPF.Chess.Movment
{
    public class Direction
    {
        public int RowDelta {  get; }
        public int ColumnDelta { get; }

        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        public static Direction operator +(Direction a, Direction b) =>
            new Direction(a.RowDelta + b.RowDelta, a.ColumnDelta + b.ColumnDelta);

        public static Direction operator *(int scalar, Direction a) =>
            new Direction(a.RowDelta * scalar, a.ColumnDelta * scalar);
    }
}
