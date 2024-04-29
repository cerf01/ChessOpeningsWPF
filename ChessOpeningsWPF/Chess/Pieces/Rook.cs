using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Rook : IPiece
    {
        public PieceType Type => PieceType.Rook;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        public Rook(PieceColor color) =>
            Color = color;
        
        public Rook(Rook rook)
        {
            Color = rook.Color;
            HasMoved = rook.HasMoved;
        }

        public IPiece Copy() =>
          new Rook(this);
    }
}
