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
    public delegate void OnMovePiece(IMove move);
    public class GameState
    {
        public BoardModel Board { get; private set; }
        public PlayerColor CurrentTurn { get;private set; }
        public PlayerColor Player;

        private string _stateString;

        private List<string> _stateHistory = new List<string>();

        private ComputerPlayer _computerPlayer;

        private Stack<BoardModel> boards = new Stack<BoardModel>();

        public static event OnMovePiece MovePiece;

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
            boards.Push(Board.Copy());
            var moveToPositions = move.MoveTo(Board );
        
            CurrentTurn = ChangeTurn();
          
            UpdateStateString();
            /*
                        CheckGameOver();
            */
            MovePiece.Invoke(move);
           
            return moveToPositions;
        }


        public void UndoMove(IMove move)
        {
            CurrentTurn = ChangeTurn();
            if (boards.Count > 0)
                Board = boards.Pop();
            MovePiece.Invoke(move);

        }

        public IMove MakeComputerMove()
        {
            return _computerPlayer.GetBestMove(this);
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
