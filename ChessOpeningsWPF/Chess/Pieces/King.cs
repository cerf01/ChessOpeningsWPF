using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class King : IPiece
    {
        public PieceType Type => PieceType.King;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        private static List<Direction> _directions => new List<Direction>
        {
            Movement.Directions.North,
            Movement.Directions.West,
            Movement.Directions.East,
            Movement.Directions.South,
            Movement.Directions.NorthEast,
            Movement.Directions.NorthWest,
            Movement.Directions.SouthEast,
            Movement.Directions.SouthWest,
        };
        public List<Direction> Directions { get => _directions; }

        public King(PieceColor color) =>
            Color = color;

        public King(King king)
        {
            Color = king.Color;
            HasMoved = king.HasMoved;
        }

        public IPiece Copy() =>
          new King(this);

        public List<Position> MovePositions(Position currPosition, BoardModel board)
        {
            var movePositions = new List<Position>();

            Position toPosition = null;

            foreach (var direction in _directions)
            {
                toPosition = currPosition + direction;

                if (BoardModel.IsInsideBoard(toPosition))
                    if (board.IsEmptySquare(toPosition) || board[toPosition].Color != Color)
                        movePositions.Add(toPosition);
            }

            return movePositions;
        }

        public List<IMove> GetMoves(Position currPosition, BoardModel board) =>
           MovePositions(currPosition, board)
                .Select(p => 
                    (IMove)new NormalMove(currPosition, p))
                .ToList();

    }
}
