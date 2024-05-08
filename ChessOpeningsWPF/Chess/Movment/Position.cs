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
    }
}
