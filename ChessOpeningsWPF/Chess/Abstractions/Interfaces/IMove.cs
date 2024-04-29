using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movment;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface IMove
    {
        Position From { get; }
        Position To { get; }
        public List<Position> MoveTo(BoardModel board);
    }
}
