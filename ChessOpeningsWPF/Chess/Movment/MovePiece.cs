using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessOpeningsWPF.Chess.Movment
{
    internal class MovePiece : IMove
    {
        public Position From {  get; }

        public Position To { get; }

        public MovePiece(Position from, Position to)
        {
            From = from;
            To = to;
        }

        public void MoveTo(BoardModel board)
        {
            IPiece piece = board[From];
            if (piece is null)
                return;
            board[To] = piece;
            board[From] = null;
            piece.HasMoved = true;
        }
    }
}
