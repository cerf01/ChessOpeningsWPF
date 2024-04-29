using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Pawn : IPiece
    {
        public PieceType Type => PieceType.Pawn;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        public Pawn(PieceColor color) =>
            Color = color;
        
        public Pawn(Pawn pawn) 
        {
            Color = pawn.Color;
            HasMoved = pawn.HasMoved;
        }

        public IPiece Copy() =>
          new Pawn(this);
        
    }
}
