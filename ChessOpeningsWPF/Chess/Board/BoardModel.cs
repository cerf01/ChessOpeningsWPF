using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Movement;
using ChessOpeningsWPF.Chess.Movement.SpecialMoves;
using ChessOpeningsWPF.Chess.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace ChessOpeningsWPF.Chess.Board
{
    public class BoardModel
    {
        private List<List<IPiece>> _squares = new List<List<IPiece>>();

        private Dictionary<PlayerColor, Position> _pawnSkiped = new Dictionary<PlayerColor, Position>()
        {
            {PlayerColor.White, null},
            {PlayerColor.Black, null}
        };

        public BoardModel()
        {
            for (int r = 0; r < 8; r++)
            {
                _squares.Add(new List<IPiece>());
                for (int c = 0; c < 8; c++)
                    _squares[r].Add(null);

            }
        }

        public IPiece this[int row, int col]
        {
            get { return _squares[row][col]; }
            set { _squares[row][col] = value; }
        }

        public IPiece this[Position position]
        {
            get { return _squares[position.Row][position.Column]; }
            set { _squares[position.Row][position.Column] = value; }
        }

        private void SetPieces()
        {

            this[0, 0] = new Rook(PlayerColor.Black, new Position(0, 0));
            this[0, 1] = new Knight(PlayerColor.Black, new Position(0, 1));
            this[0, 2] = new Bishop(PlayerColor.Black, new Position(0, 2));
            this[0, 3] = new Queen(PlayerColor.Black, new Position(0, 3));
            this[0, 4] = new King(PlayerColor.Black, new Position(0, 4));
            this[0, 5] = new Bishop(PlayerColor.Black, new Position(0, 5));
            this[0, 6] = new Knight(PlayerColor.Black, new Position(0, 6));
            this[0, 7] = new Rook(PlayerColor.Black, new Position(0, 7));

            this[7, 0] = new Rook(PlayerColor.White, new Position(7, 0));
            this[7, 1] = new Knight(PlayerColor.White, new Position(7, 1));
            this[7, 2] = new Bishop(PlayerColor.White, new Position(7, 2));
            this[7, 3] = new Queen(PlayerColor.White, new Position(7, 3));
            this[7, 4] = new King(PlayerColor.White, new Position(7, 4));
            this[7, 5] = new Bishop(PlayerColor.White, new Position(7, 5));
            this[7, 6] = new Knight(PlayerColor.White, new Position(7, 6));
            this[7, 7] = new Rook(PlayerColor.White, new Position(7, 7));


            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(PlayerColor.Black, new Position(1, c));
                this[6, c] = new Pawn(PlayerColor.White, new Position(6, c));
            }

        }

        public static BoardModel InitialBoard()
        {
            BoardModel board = new BoardModel();

            board.SetPieces();

            return board;
        }

        public Position GetPawnSkipedPositions(PlayerColor color) =>
            _pawnSkiped[color];

        public void SetPawnSkipedPosition(PlayerColor color, Position position) =>
            _pawnSkiped[color] = position;


        public static bool IsInsideBoard(Position position) =>
            position.Row >= 0 && position.Column >= 0 && position.Row < 8 && position.Column < 8;

        public bool IsEmptySquare(Position position) =>
            this[position] is null;

        public List<Position> PiecePositions()
        {
            var positions = new List<Position>();

            Position position = null;

            for (int r = 0; r < _squares.Count; r++)
                for (int c = 0; c < _squares[r].Count; c++)
                {
                    position = new Position(r, c);

                    if (!IsEmptySquare(position))
                        positions.Add(position);
                }


            return positions;
        }

        public List<Position> PiecePositionsByColor(PlayerColor color) =>
            PiecePositions()
                .Where(p =>
                    this[p].Color == color)
                .ToList();

        public bool IsInCheck(PlayerColor color) =>
            PiecePositionsByColor(color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White)
                .Any(p =>
                    this[p]
                    .CanCaptureEnemyKing(p, this));

        public BoardModel Copy()
        {
            var board = new BoardModel();

            foreach (var position in PiecePositions())
                board[position] = this[position].Copy();

            return board;
        }

        private bool IsKingRookMoved(Position kingPosition, Position rookPosition)
        {
            if (IsEmptySquare(kingPosition) || IsEmptySquare(rookPosition))
                return false;

            return this[kingPosition].HasMoved && this[rookPosition].HasMoved;
        }

        public bool CanCaslteRightSide(PlayerColor color) =>
            color == PlayerColor.White? IsKingRookMoved(new Position(7,4), new Position(7, 7)): IsKingRookMoved(new Position(0, 4), new Position(0, 4));

        public bool CanCaslteLeftSide(PlayerColor color) =>
            color == PlayerColor.White ? IsKingRookMoved(new Position(7, 4), new Position(7, 0)) : IsKingRookMoved(new Position(0, 4), new Position(0, 0));

        private bool HasPawnInPosition(PlayerColor color, List<Position> pawnPositions, Position skipPosition)
        {
            IPiece piece = null;
                
            foreach (var position in pawnPositions.Where(IsInsideBoard))
            {
                piece = this[position];
                if (piece is not null|| piece.Type != PieceType.Pawn || piece.Color != color)
                    continue;
                    
                var move = new EnPassant(position, skipPosition);

                if (move.IsLegal(this)) 
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanMakeEnPassant(PlayerColor color)
        { 
            var skipedPositions = GetPawnSkipedPositions(color);
            if(skipedPositions is null) 
                return false;

            var pawnPositions = color == PlayerColor.White ? new List<Position>() { skipedPositions + Directions.SouthWest, skipedPositions + Directions.SouthEast } : new List<Position>() { skipedPositions + Directions.NorthWest, skipedPositions + Directions.NorthEast };

            return HasPawnInPosition(color, pawnPositions, skipedPositions);

        }

    }
}
