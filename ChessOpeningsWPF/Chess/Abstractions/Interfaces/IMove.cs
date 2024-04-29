using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movment;

namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface IMove
    {
        Position From { get; }
        Position To { get; }
        public void MoveTo(BoardModel board);
    }
}
