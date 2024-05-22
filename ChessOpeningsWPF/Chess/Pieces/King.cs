using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Board.Movement;
using ChessOpeningsWPF.Chess.Board.Movement.SpecialMoves;
using System.Collections.Generic;
using System.Linq;


namespace ChessOpeningsWPF.Chess.Pieces
{
    public class King : IPiece
    {
        public PieceType Type => PieceType.King;

        public PlayerColor Color { get; }

        public bool HasMoved { get; set; } = false;

        private static List<Direction> _directions => new List<Direction>
        {
            Board.Movement.Directions.North,
            Board.Movement.Directions.West,
            Board.Movement.Directions.East,
            Board.Movement.Directions.South,
            Board.Movement.Directions.NorthEast,
            Board.Movement.Directions.NorthWest,
            Board.Movement.Directions.SouthEast,
            Board.Movement.Directions.SouthWest,
        };
        public List<Direction> Directions { get => _directions; }
        public Position Position { get; set; }

        public int Value => 2000;

        public King(PlayerColor color, Position position)
        {
            Color = color;
            Position = position;
        }

        public King(King king)
        {
            Color = king.Color;
            HasMoved = king.HasMoved;
            Position = king.Position;

        }

        public IPiece Copy() =>
          new King(this);

        private bool IsRookMoved(Position position, BoardModel board)
        {
            if (board.IsEmptySquare(position))
                return true;

            return (board[position].Type == PieceType.Rook && board[position].HasMoved);
        }

        private bool IsPathClear(List<Position> positions, BoardModel board) =>
            positions.All(p => board.IsEmptySquare(p));

        private bool IsRightCastlingPossible(Position position, BoardModel board)
        {
            if (HasMoved)
                return false;
            var positions = new List<Position>()
            {
                new Position(position.Row, 5),
                new Position(position.Row, 6)
            };

            return IsPathClear(positions, board) && !IsRookMoved(new Position(position.Row, 7), board); ;
        }

        private bool IsLeftCastlingPossible(Position position, BoardModel board)
        {
            if (HasMoved)
                return false;

            var positions = new List<Position>()
            {
                new Position(position.Row, 1),
                new Position(position.Row, 2),
                new Position(position.Row, 3)
            };

            return IsPathClear(positions, board) && !IsRookMoved(new Position(position.Row, 0), board);
        }

        public List<Position> MovesPositions(Position currPosition, BoardModel board)
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

        public List<IMove> GetMoves(Position currPosition, BoardModel board)
        {
          var moves = MovesPositions(currPosition, board)
               .Select(p =>
                   (IMove)new NormalMove(currPosition, p))
               .ToList();
            if (IsLeftCastlingPossible(currPosition, board))
                moves.Add(new Castling(MoveType.CastleL, currPosition));
            if (IsRightCastlingPossible(currPosition, board))
                moves.Add(new Castling(MoveType.CastleR, currPosition));

            return moves;
        }
        public bool CanCaptureEnemyKing(Position position, BoardModel board) =>
            MovesPositions(position, board).Any(p =>
                board[p] is not null &&
                board[p].Type == PieceType.King
            );
        public bool CanCaptureEnemy(Position position, BoardModel board) =>
            MovesPositions(position, board).Any(p => board[p] is not null);
    }
}
