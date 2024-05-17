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

        public PlayerColor Player { get; private set; }

        private string _stateString;

        private Stack<string> _stateHistory = new Stack<string>();

        private Stack<BoardModel> _boardHystory = new Stack<BoardModel>();

        public Stack<IPiece> CapchuredPieces = new Stack<IPiece>();

        private ComputerPlayer _computerPlayer;

        public static event OnMovePiece MovePiece;

    
        public GameState(PlayerColor playerColor, BoardModel board)
        {
            Player = playerColor;

            CurrentTurn = PlayerColor.White;

            _computerPlayer = new ComputerPlayer(4);

            Board = board;

            _stateString = new FENString(CurrentTurn, Board).ToString();

            _stateHistory.Push(_stateString);
        }

        public void ResetBoard() 
            => Board = BoardModel.InitialBoard(_stateString);

        private PlayerColor ChangeTurn() =>
            CurrentTurn == PlayerColor.White ? PlayerColor.Black: PlayerColor.White;

        public List<Position> MakeMove(IMove move)
        {

            Board.SetPawnSkipedPosition(CurrentTurn, null);
            _boardHystory.Push(Board.Copy());

            if (!Board.IsEmptySquare(move.To))
                CapchuredPieces.Push(Board[move.To]);

            var moveToPositions = move.MoveTo(Board);
            
            CurrentTurn = ChangeTurn();
          
            UpdateStateString();
           
            return moveToPositions;
        }

        public void UndoMove(IMove move)
        {
            CurrentTurn = ChangeTurn();
            
            _stateHistory.Pop();
            if(CapchuredPieces.Count > 0)
                CapchuredPieces.Pop();            

            if (_boardHystory.Count > 0)
                Board = _boardHystory.Pop();

        }

        public IMove MakeComputerMove() =>
            _computerPlayer.GetBestMove(this);
        

        private void CheckGameOver() 
        {
            if(!Board.AllLegalPlayerMoves(CurrentTurn).Any())          
                MessageBox.Show("Game over!");
            
        }

        private void UpdateStateString()
        {
            _stateString = new FENString(CurrentTurn, Board).ToString();
            _stateHistory.Push(_stateString);
        }
    }
}
