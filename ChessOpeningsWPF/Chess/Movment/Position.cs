using ChessOpeningsWPF.Chess.Abstractions.Enums;
using System;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Movment
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public PieceColor SqueareColor()
        {
            if ((Row + Column) % 2 == 0)
                return PieceColor.White;
            
           return PieceColor.Black;
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Row, Column);
        
        public static bool operator ==(Position? left, Position? right) =>
            EqualityComparer<Position>.Default.Equals(left, right);
        
        public static bool operator !=(Position? left, Position? right) =>
            !(left == right);
        
        public static Position operator +(Position position, Direction direction) =>
            new Position(position.Row + direction.RowDelta, position.Column + direction.ColumnDelta);
    }
}
