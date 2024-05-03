using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface IPiece
    {
        PieceType Type { get; }

        PieceColor Color { get; }

        bool HasMoved { get; set; }

        public List<Direction> Directions { get; }

        public IPiece Copy();
       
        public List<IMove> GetMoves(Position currPosition, BoardModel board);

       
    }
}
