using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Queen : IPiece
    {
        public PieceType Type => PieceType.Queen;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        public Queen(PieceColor color) =>
          Color = color;

        public Queen(Queen queen)
        {
            Color = queen.Color;
            HasMoved = queen.HasMoved;
        }

        public IPiece Copy() =>
          new Queen(this);
    }
}
