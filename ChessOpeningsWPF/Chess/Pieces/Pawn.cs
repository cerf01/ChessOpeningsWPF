using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Pieces
{
    public class Pawn : IPiece
    {
        public PieceType Type => PieceType.Pawn;

        public PieceColor Color { get; }

        public bool HasMoved { get; set; } = false;

        private List<Direction> _directions => new List<Direction>()
        {
            Movement.Directions.North,
            Movement.Directions.South,
            Movement.Directions.East,
            Movement.Directions.West,
        };

        public List<Direction> Directions { get => _directions; }

        private readonly Direction _forward;

        public Pawn(PieceColor color) 
        {   Color = color;
            
            _forward = Color == PieceColor.White ? _directions[0] : _directions[1];
        }
        public Pawn(Pawn pawn)
        {
            Color = pawn.Color;

            _forward = Color == PieceColor.White ? _directions[0] : _directions[1];

            HasMoved = pawn.HasMoved;
        }

        public IPiece Copy() =>
          new Pawn(this);

        private bool CanMoveTo(Position position, BoardModel board) =>
            BoardModel.IsInsideBoard(position) && board.IsEmptySquare(position);

        private bool CanCaptureAt(Position position, BoardModel board)
        {
            if (!BoardModel.IsInsideBoard(position) || board.IsEmptySquare(position))
                return false;

            return board[position].Color != Color;
        }

        private List<IMove> ForvardMoves(Position currPosition, BoardModel board)
        {
            var moves = new List<IMove>();

            var oneMove = currPosition + _forward;

            if (CanMoveTo(oneMove, board))
            {
                moves.Add(new NormalMove(currPosition, oneMove));

                var twoMove = oneMove + _forward;

                if (!HasMoved && CanMoveTo(twoMove, board))
                    moves.Add(new NormalMove(currPosition, twoMove));
            }

            return moves;
        }

        private List<IMove> AttackMoves(Position currPosition, BoardModel board)
        {
            var moves = new List<IMove>();

            Position toPosition = null;

            for (int i = 2; i < _directions.Count; i++)
            {
                toPosition = currPosition + _forward + _directions[i];
                if (CanCaptureAt(toPosition, board))
                    moves.Add(new NormalMove(currPosition, toPosition));
            }

            return moves;
        }

        public List<IMove> GetMoves(Position currPosition, BoardModel board) =>
            ForvardMoves(currPosition, board)
                .Union(AttackMoves(currPosition, board))
                .ToList();       
    }
}
