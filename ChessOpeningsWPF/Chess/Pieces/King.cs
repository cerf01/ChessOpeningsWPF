using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class King : IPiece
    {
        public PieceType Type => PieceType.King;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        public King(PieceColor color) =>
            Color = color;

        public King(King king)
        {
            Color = king.Color;
            HasMoved = king.HasMoved;
        }

        public IPiece Copy() =>
          new King(this);
    }
}
