using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Bishop : IPiece
    {
        public PieceType Type => PieceType.Bishop;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        public Bishop(PieceColor color) =>
            Color = color;
        
        public Bishop(Bishop bishop)
        {
            Color = bishop.Color;
            HasMoved = bishop.HasMoved;
        }

        public IPiece Copy() =>
          new Bishop(this);
    }
}
