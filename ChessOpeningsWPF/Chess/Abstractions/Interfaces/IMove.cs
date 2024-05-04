using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface IMove
    {
        MoveType Type { get; }
        Position From { get; }
        Position To { get; }
        public List<Position> MoveTo(BoardModel board);
        public bool IsLegal(BoardModel board);
    }
}
