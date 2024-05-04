using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Movement.SpecialMoves
{
    public class Castling : IMove
    {
        public MoveType Type { get; }
        public Position From { get; }
        public Position To { get; }

        private Direction _kingDirection;

        private Position _rookFrom;

        private Position _rookTo;

        public Castling(MoveType type, Position from)
        {
            Type = type;

            From = from;

            if (type == MoveType.CastleR)
            {
                _kingDirection = Directions.East;
                To = new Position(from.Row, 6);
                _rookFrom = new Position(from.Row, 7);
                _rookTo = new Position(from.Row, 5);
            }
            else 
            {
                _kingDirection = Directions.West;
                To = new Position(from.Row, 2);
                _rookFrom = new Position(from.Row, 0);
                _rookTo = new Position(from.Row, 3);
            }
          
        }

        public List<Position> MoveTo(BoardModel board)
        {
            var kingPiece = board[From];

            var rookPiece = board[_rookFrom];

            if (kingPiece is null && rookPiece is null)
                return null;

            new NormalMove(From, To).MoveTo(board);
            new NormalMove(_rookFrom, _rookTo).MoveTo(board);

            return new List<Position>() { From, To, _rookFrom, _rookTo };
        }

        public bool IsLegal(BoardModel board)
        {
            var color = board[From].Color;

            if (board.IsInCheck(color))
                return false;

            var boardCopy = board.Copy();

            Position kingPosCopy = From;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPosCopy, kingPosCopy + _kingDirection).MoveTo(boardCopy);
                if (board.IsInCheck(color))
                    return false;
            }
            return true;
        }
    }
}
