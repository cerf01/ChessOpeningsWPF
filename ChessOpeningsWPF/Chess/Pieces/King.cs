using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using ChessOpeningsWPF.Chess.Movement.SpecialMoves;
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


        private bool IsPathClear(List<Position> positions, BoardModel board) =>
            positions.All(p => board.IsEmptySquare(p));
        
        private bool IsRightCastlingPossible(Position position, BoardModel board) 
        {     
            var rook = board[position.Row, 7];

            if (HasMoved || rook.HasMoved)
                return false;

            var positions = new List<Position>() { new Position(position.Row, 5), new Position(position.Row, 6) };

            return IsPathClear(positions, board);
        }

        private bool IsLeftCastlingPossible(Position position, BoardModel board)
        {
            var rook = board[position.Row, 0];

            if (HasMoved || rook.HasMoved)
                return false;

            var positions = new List<Position>() { new Position(position.Row, 1), new Position(position.Row, 2), new Position(position.Row, 3) };

            return IsPathClear(positions, board);
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
            var moves = new List<IMove>();

            if(IsLeftCastlingPossible(currPosition, board))
                moves.Add(new Castle(MoveType.CastleL, currPosition));
            if (IsRightCastlingPossible(currPosition, board)) 
                moves.Add(new Castle(MoveType.CastleR, currPosition));

            moves.AddRange(MovesPositions(currPosition, board)
                 .Select(p =>
                     (IMove)new NormalMove(currPosition, p))
                 .ToList());
           
            return moves;
        }

        public bool CanCaptureEnemyKing(Position position, BoardModel board) =>
            MovesPositions(position, board).Any(p =>
                board[p] is not null &&
                board[p].Type == PieceType.King
            );


    }
}
