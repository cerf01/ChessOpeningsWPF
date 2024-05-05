using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Movement.SpecialMoves
{
    public class DoubleMove : IMove
    {
        public MoveType Type => MoveType.DoublePawn;

        public Position From { get; }

        public Position To { get; }

        private Position _skiped;

        public DoubleMove(Position from, Position to)
        {
            From = from;
            To = to;
            _skiped = new Position((from.Row+to.Row)/2, from.Column);
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
            if (board[From] is null)
                return new List<Position>();
            var pieceColor = board[From].Color;


            board.SetPawnSkipedPosition(pieceColor, _skiped);

            var moves = new NormalMove(From, To).MoveTo(board);

            return moves;
        }
    }
}
