using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Queen : IPiece, ILongStepPiece
    {
        public PieceType Type => PieceType.Queen;

        public PlayerColor Color { get; }

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
        public Position Position { get; set; }

        public Queen(PlayerColor color, Position position) 
         {
                Color = color;
                Position = position;
            }

    public Queen(Queen queen)
        {
            Color = queen.Color;
            HasMoved = queen.HasMoved;
            Position =  queen.Position;
        }

        public IPiece Copy() =>
          new Queen(this);
      
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

        public List<IMove> GetMoves(Position currPosition, BoardModel board) =>
          MoveDirections(currPosition, _directions, board)
              .Select(to => (IMove)new NormalMove(currPosition, to))
              .ToList();

        public bool CanCaptureEnemyKing(Position position, BoardModel board) =>
           GetMoves(position, board).Any(m =>
               board[m.To] is not null &&
               board[m.To].Type == PieceType.King
           );
    }
}
