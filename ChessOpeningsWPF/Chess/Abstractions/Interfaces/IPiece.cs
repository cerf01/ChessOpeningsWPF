using ChessOpeningsWPF.Chess.Abstractions.Enums;

namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface IPiece
    {
        PieceType Type { get; }

        PieceColor Color { get; }

        bool HasMoved { get; set; }

        public IPiece Copy();

    }
}
