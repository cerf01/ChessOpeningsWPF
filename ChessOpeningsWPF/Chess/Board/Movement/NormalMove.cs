using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Board.Movement
{
    public class NormalMove : IMove
    {
        public Position From {  get; }
        public Position To { get; }
        public MoveType Type { get; }
        public NormalMove(Position from, Position to)
        {
            Type = MoveType.Normal;
            From = from;
            To = to;
        }

        public List<Position> MoveTo(BoardModel board)
        {
            var piece = board[From];
            
            if (piece is null)
                return null;

           
            board[From] = null;

            board[To] = piece;
            piece.Position = To;

            piece.HasMoved = true;
            
            return new List<Position>() { From, To };
        }

        public bool IsLegal(BoardModel board)
        {
            var color = board[From].Color;

            var boardCopy = board.Copy();

            MoveTo(boardCopy);
           
            
            return !boardCopy.IsInCheck(color);
        }
    }
}
