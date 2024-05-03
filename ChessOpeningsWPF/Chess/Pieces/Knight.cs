using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Knight : IPiece
    {
        public PieceType Type => PieceType.Knight;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        private static List<Direction> _directions = new List<Direction>()
        {
            Movement.Directions.South,
            Movement.Directions.North,
            Movement.Directions.West,
            Movement.Directions.East,
        };

        public List<Direction> Directions { get => _directions; }

        public Knight(PieceColor color) =>
            Color = color;

        public Knight(Knight knight)
        {
            Color = knight.Color;
            HasMoved = knight.HasMoved;
        }

        public IPiece Copy() =>
          new Knight(this);

        private static List<Position> PossiblePositions(Position currPosition) 
        {
            var positions = new List<Position>();

            for (int v = 0; v < _directions.Count/2; v++)
                for (int h = _directions.Count / 2; h < _directions.Count; h++)
                {
                    positions.Add(currPosition + 2 * _directions[v] + _directions[h]);
                    positions.Add(currPosition + 2 * _directions[h] + _directions[v]);
                }
            return positions;
        }

        public List<Position> MovePositions(Position currPosition, BoardModel board) =>
            PossiblePositions(currPosition)
                             .Where(p =>
                                BoardModel.IsInsideBoard(p)
                                && (board.IsEmptySquare(p) || board[p].Color != Color))
                             .ToList();
        
        public List<Position> MoveDirections(Position currPosition, List<Direction> directions, BoardModel board) =>
            directions.SelectMany(d => 
                            MovePositions(currPosition, board))
                        .ToList();

        public List<IMove> GetMoves(Position currPosition, BoardModel board) =>
            MovePositions(currPosition, board)
                .Select(p => (IMove)new NormalMove(currPosition, p))
                .ToList();

    }
}
