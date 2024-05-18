using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board.Movement;
using ChessOpeningsWPF.Chess.Board.Movement.SpecialMoves;
using ChessOpeningsWPF.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
            InitSquares();
        }

        private void InitSquares()
        {
            for (int r = 0; r < 8; r++)
            {
                _squares.Add(new List<IPiece>());
                for (int c = 0; c < 8; c++)
                {
                    _squares[r].Add(null);
                }
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

        private IPiece? GetPieceFormChar(char pieceChar, PlayerColor color, int rank, int file)
        {
            switch (pieceChar)
            {
                case 'p':
                    return new Pawn(color, new Position(rank, file));
                case 'b':
                    return new Bishop(color, new Position(rank, file));
                case 'r':
                    return new Rook(color, new Position(rank, file));
                case 'q':
                    return new Queen(color, new Position(rank, file));
                case 'k':
                    return new King(color, new Position(rank, file));
                case 'n':
                    return new Night(color, new Position(rank, file));
                default:
                    return null;
            }
        }

        public void SetPiecesFromFEN(string fenString)
        {
            var str = fenString.Split(' ')[0];

            int rank = 0;
            int file = 0;

            _squares.Clear();
            InitSquares();

            foreach (var symbol in str)
            {
                if (symbol == '/')
                {
                    rank++;
                    file = 0;

                }
                else
                {
                    if (char.IsDigit(symbol))
                        file += (int)char.GetNumericValue(symbol);
                    else
                    {
                        var pieceColor = char.IsLower(symbol) ? PlayerColor.Black : PlayerColor.White;
                        _squares[rank][file] = GetPieceFormChar(char.ToLower(symbol), pieceColor, rank, file);
                        file++;
                    }
                }
            }
        }

        public bool PawnPromoted(Position position)
        {
            if (this[position].Type == PieceType.Pawn)
            {
                if(this[position].Color == PlayerColor.White && position.Row == 0 || this[position].Color == PlayerColor.Black && position.Row == 7)
                this[position] = new Queen(this[position].Color, this[position].Position);                
                return true;
            }

            return false;
        }

        public List<IMove> AvailableMovesForPiece(Position position, PlayerColor currentTurn)
        {
            if (!IsInsideBoard(position) || this[position].Color != currentTurn)
                return new List<IMove>();

            var possibleMoves = this[position].GetMoves(position, this);

            return possibleMoves.Where(m => m.IsLegal(this)).ToList();
        }

        public List<IMove> AllLegalPlayerMoves(PlayerColor color) =>
            PiecePositionsByColor(color)
               .SelectMany(p =>
                   this[p].GetMoves(p, this))
               .Where(m => m.IsLegal(this))
               .ToList();

        public List<IMove> AllCapturePlayerMoves(PlayerColor color)
        {
           var moves = PiecePositionsByColor(color)
            .SelectMany(p =>
                this[p].GetMoves(p, this))
            .Where(m => m.IsLegal(this))
            .ToList();

            var captureMoves = new List<IMove>();

            foreach (var move in moves)
            {
                if (!IsEmptySquare(move.To))
                    if (this[move.To].Color != color)
                        captureMoves.Add(move);
            }

            return captureMoves;
        }

        public static BoardModel InitialBoard(string fenString)
        {
            BoardModel board = new BoardModel();

            board.SetPiecesFromFEN(fenString);
           
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

        public bool IsInAttack(PlayerColor color, Position position) 
        { 
         var squarsOnAttack = PiecePositionsByColor(color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White)
             .Where(p => this[p].CanCaptureEnemy(p, this));
            return squarsOnAttack.Contains(position);
        }
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
                if (piece is null|| piece.Type != PieceType.Pawn || piece.Color != color)
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
