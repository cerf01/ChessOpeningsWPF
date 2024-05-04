using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChessOpeningsWPF.Chess
{
    public class GameState
    {
        public BoardModel Board { get; private set; }
        public PlayerColor CurrentPlayerTurn { get;private set; }
        private string _stateString;
        private Dictionary<string, int> _stateHistory = new Dictionary<string, int>();

        public GameState(PlayerColor currentColorTurn, BoardModel board)
        {
            CurrentPlayerTurn = currentColorTurn;
            Board = board;

            _stateString = new StateString(currentColorTurn, board).ToString();

            _stateHistory[_stateString] = 1;
        }

        public List<IMove> AvailableMovesForPiece(Position position)
        {
            if (!BoardModel.IsInsideBoard(position) || Board[position].Color != CurrentPlayerTurn)
                return new List<IMove>();

            var possibleMoves = Board[position].GetMoves(position, Board);

            return possibleMoves.Where(m => m.IsLegal(Board)).ToList();
        }

        public void ResetBoard() 
            => Board = BoardModel.InitialBoard();

        private PlayerColor ChangeTurn() =>
            CurrentPlayerTurn == PlayerColor.White ? PlayerColor.Black: PlayerColor.White;

        public List<Position> MakeMove(IMove move)
        {
            Board.SetPawnSkipedPosition(CurrentPlayerTurn, null);

            var moveToPositions = move.MoveTo(Board);
            
            CurrentPlayerTurn = ChangeTurn();

            UpdateStateString();

            CheckGameOver();

            return moveToPositions;
        }

        public List<IMove> AllLegalPlayerMoves(PlayerColor color) =>
            Board.PiecePositionsByColor(color)
                .SelectMany(p => 
                    Board[p].GetMoves(p, Board))
                .Where(m => m.IsLegal(Board))
                .ToList();

        private void CheckGameOver() 
        {
            if(!AllLegalPlayerMoves(CurrentPlayerTurn).Any())          
                MessageBox.Show("Game over!");
            
        }

        private void UpdateStateString()
        {
            _stateString = new StateString(CurrentPlayerTurn, Board).ToString();
           
        }
    }
}
