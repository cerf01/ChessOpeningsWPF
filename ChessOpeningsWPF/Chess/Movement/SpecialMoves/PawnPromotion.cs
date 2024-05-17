using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Pieces;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Movement.SpecialMoves
{
    public class PawnPromotion : IMove
    {
        public MoveType Type => MoveType.PawnPromotion;

        public Position From { get; }

        public Position To { get; }

        private PieceType _newPieceType;

        public PawnPromotion(Position from, Position to, PieceType newPieceType)
        {
            From = from;
            To = to;
            _newPieceType = newPieceType;
        }

        private IPiece CreatePromotionPiece(PlayerColor color)
        {
            switch (_newPieceType)
            {
                case PieceType.Night:
                    return new Night(color, To);
                case PieceType.Bishop:
                    return new Bishop(color, To);
                case PieceType.Rook:
                    return new Rook(color, To);
                default:
                    return new Queen(color, To);
            }
        }
        public List<Position> MoveTo(BoardModel board)
        {
            var piece = board[From];

            if (piece is null)
                return null;

            var newPiece = CreatePromotionPiece(piece.Color);

            board[To] = newPiece;
            board[From] = null;

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
