using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess
{
    public class GameState
    {
        public BoardModel Board { get; private set; }
        public PieceColor CurrentColorTurn { get;private set; }

        public GameState(PieceColor currentColorTurn, BoardModel board)
        {
            CurrentColorTurn = currentColorTurn;
            Board = board;
        }
        public List<IMove> PossibleMovesForPiece(Position position)
        {
            if (!BoardModel.IsInsideBoard(position) || Board[position].Color != CurrentColorTurn)
                return new List<IMove>();

            return Board[position].GetMoves(position, Board);
            

        }

        public void ResetBoard() 
            => Board = BoardModel.InitialBoard();

        private PieceColor ChangeColor() =>
            CurrentColorTurn == PieceColor.White ? PieceColor.Black: PieceColor.White;

        public List<Position> MakeMove(IMove move)
        {
            var moveToPositions = move.MoveTo(Board);
            CurrentColorTurn = ChangeColor();

            return moveToPositions;
        }
    }
}
