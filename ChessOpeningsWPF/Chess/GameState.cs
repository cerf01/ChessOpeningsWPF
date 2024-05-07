using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using ChessOpeningsWPF.Chess.SortOfChessAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChessOpeningsWPF.Chess
{
    public class GameState
    {
        public BoardModel Board { get; private set; }
        public PlayerColor CurrentTurn { get;private set; }
        public PlayerColor Player;
        private string _stateString;
        private Dictionary<string, int> _stateHistory = new Dictionary<string, int>();
        private ComputerPlayer _computerPlayer;

        public GameState(PlayerColor playerColor, BoardModel board)
        {
            Player = playerColor;

            CurrentTurn = PlayerColor.White;

            _computerPlayer = new ComputerPlayer(2, Player==PlayerColor.White? PlayerColor.Black: PlayerColor.White);

            Board = board;

            _stateString = new FENString(CurrentTurn, Board).ToString();

            _stateHistory[_stateString] = 1;
        }

        public List<IMove> AvailableMovesForPiece(Position position)
        {
            if (!BoardModel.IsInsideBoard(position) || Board[position].Color != CurrentTurn)
                return new List<IMove>();

            var possibleMoves = Board[position].GetMoves(position, Board);

            return possibleMoves.Where(m => m.IsLegal(Board)).ToList();
        }

        public void ResetBoard() 
            => Board = BoardModel.InitialBoard(_stateString);

        private PlayerColor ChangeTurn() =>
            CurrentTurn == PlayerColor.White ? PlayerColor.Black: PlayerColor.White;

        public List<Position> MakeMove(IMove move)
        {
            Board.SetPawnSkipedPosition(CurrentTurn, null);

            var moveToPositions = move.MoveTo(Board);
            
            CurrentTurn = ChangeTurn();

            UpdateStateString();

            CheckGameOver();

            return moveToPositions;
        }

        public IMove MakeComputerMove()
        {
                    return _computerPlayer.GetBestMove(Board, this);
        }

        public List<IMove> AllLegalPlayerMoves(PlayerColor color) =>
            Board.PiecePositionsByColor(color)
                .SelectMany(p => 
                    Board[p].GetMoves(p, Board))
                .Where(m => m.IsLegal(Board))
                .ToList();

        private void CheckGameOver() 
        {
            if(!AllLegalPlayerMoves(CurrentTurn).Any())          
                MessageBox.Show("Game over!");
            
        }

        private void UpdateStateString()
        {
            _stateString = new FENString(CurrentTurn, Board).ToString();
            _stateHistory[_stateString] = _stateHistory.Count + 1;
        }
    }
}
