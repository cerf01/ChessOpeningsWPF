using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Board.Movement;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Bishop : IPiece, ILongStepPiece
    {
        public PieceType Type => PieceType.Bishop;

        public PlayerColor Color { get; }

        private static List<Direction> _directions => new List<Direction>
        {
            Board.Movement.Directions.NorthEast,
            Board.Movement.Directions.NorthWest,
            Board.Movement.Directions.SouthEast,
            Board.Movement.Directions.SouthWest,
        };

        public bool HasMoved { get; set; } = false;

        public List<Direction> Directions { get => _directions; }
        public Position Position { get; set; }

        public int Value => 330;

        public Bishop(PlayerColor color, Position position) 
            {
                Color = color;
                Position = position;
            }

        public Bishop(Bishop bishop)
        {
            Color = bishop.Color;
            HasMoved = bishop.HasMoved;
            Position = bishop.Position;

        }

        public IPiece Copy() =>
          new Bishop(this);

        public List<IMove> GetMoves(Position currPosition, BoardModel board) =>
           MoveDirections(currPosition, _directions, board).Select(to => (IMove)new NormalMove(currPosition, to))
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
            directions.SelectMany(d 
                    => MovePositions(currPosition, d, board))
                .ToList();

        public bool CanCaptureEnemyKing(Position position, BoardModel board) =>
           GetMoves(position, board).Any(m =>
               board[m.To] is not null &&
               board[m.To].Type == PieceType.King
           );

        public bool CanCaptureEnemy(Position position, BoardModel board) =>
            GetMoves(position, board).Any(m => board[m.To] is not null);
    }
}
