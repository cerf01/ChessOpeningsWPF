using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Board.Movement.SpecialMoves
{
    public class EnPassant : IMove
    {
        public MoveType Type => MoveType.EnPassant;

        public Position From { get; }

        public Position To { get; }

        private Position _captured;

        public EnPassant(Position from, Position to)
        {
            From = from;
            To = to;
            _captured = new Position(from.Row, to.Column);
        }

        public bool IsLegal(BoardModel board)
        {
            var color = board[From].Color;

            var boardCopy = board.Copy();

            MoveTo(boardCopy);

            return !boardCopy.IsInCheck(color);
        }

        public List<Position> MoveTo(BoardModel board)
        {
            var moves = new NormalMove(From, To).MoveTo(board);

            board[_captured] = null;

            moves.Add(_captured);

            return moves;
        }
    }
}
