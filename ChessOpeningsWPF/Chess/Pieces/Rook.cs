using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Rook : IPiece
    {
        public PieceType Type => PieceType.Rook;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        private static List<Direction> _directions => new List<Direction>
        {
            Movement.Directions.North,
            Movement.Directions.West,
            Movement.Directions.East,
            Movement.Directions.South,
        };

        public List<Direction> Directions { get => _directions; }

        public Rook(PieceColor color) =>
            Color = color;

        public Rook(Rook rook)
        {
            Color = rook.Color;
            HasMoved = rook.HasMoved;
        }

        public IPiece Copy() =>
          new Rook(this);


        public List<IMove> GetMoves(Position currPosition, BoardModel board) =>
            MoveDirections(currPosition, _directions, board)
                .Select(to => (IMove)new NormalMove(currPosition, to))
                .ToList();

        public List<Position> MovePositions(Position currPosition, Direction direction, BoardModel board)
        {
            var movePositions = new List<Position>();
            for (var pos = currPosition + direction; BoardModel.IsInsideBoard(pos); pos += direction)
            {
                if (board.IsEmptySquare(pos))
                {
                    movePositions.Add(pos);
                    continue;
                }
                if (board[pos].Color != Color)
                  movePositions.Add(pos);
               
                break;
            }
            
            return movePositions;
        }

        public List<Position> MoveDirections(Position currPosition, List<Direction> directions, BoardModel board) =>
            directions.SelectMany(d => 
                    MovePositions(currPosition, d, board))
                .ToList();
    }
}
