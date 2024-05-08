using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using ChessOpeningsWPF.Chess.SortOfChessAI;
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

        private List<string> _stateHistory = new List<string>();

        private ComputerPlayer _computerPlayer;

        public GameState(PlayerColor playerColor, BoardModel board)
        {
            Player = playerColor;

            CurrentTurn = PlayerColor.White;

            _computerPlayer = new ComputerPlayer(4);

            Board = board;

            _stateString = new FENString(CurrentTurn, Board).ToString();

            _stateHistory.Add(_stateString);
        }

        public void ResetBoard() 
            => Board = BoardModel.InitialBoard(_stateString);

        private PlayerColor ChangeTurn() =>
            CurrentTurn == PlayerColor.White ? PlayerColor.Black: PlayerColor.White;

        public List<Position> MakeMove(IMove move)
        {

            Board.SetPawnSkipedPosition(CurrentTurn, null);

            var moveToPositions = move.MoveTo(Board );

            CurrentTurn = ChangeTurn();
          
            UpdateStateString();

            CheckGameOver();

            return moveToPositions;
        }

        public IMove MakeComputerMove()
        {
            return _computerPlayer.GetbestMove(this);
        }

        private void CheckGameOver() 
        {
            if(!Board.AllLegalPlayerMoves(CurrentTurn).Any())          
                MessageBox.Show("Game over!");
            
        }

        private void UpdateStateString()
        {
            _stateString = new FENString(CurrentTurn, Board).ToString();
            _stateHistory.Add(_stateString);
        }
    }
}
