using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Knight : IPiece
    {
        public PieceType Type => PieceType.Knight;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        public Knight(PieceColor color) =>
            Color = color;

        public Knight(Knight knight)
        {
            Color = knight.Color;
            HasMoved = knight.HasMoved;
        }

        public IPiece Copy() =>
          new Knight(this);
    }
}
