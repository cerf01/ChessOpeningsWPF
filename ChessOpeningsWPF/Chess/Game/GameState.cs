﻿using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Board.Movement;
using ChessOpeningsWPF.Chess.Game.SortOfChessAI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChessOpeningsWPF.Chess.Game
{
    public class GameState
    {
        public BoardModel Board { get; private set; }
        public PlayerColor CurrentTurn { get;private set; }
        public GameResult Result { get; private set; }
        public PlayerColor Player { get; private set; }
        private string _stateString;
        private Stack<string> _stateHistory;
        private Stack<BoardModel> _boardHystory ;
        public Stack<IPiece> CapchuredPieces ;
        private ComputerPlayer _computerPlayer;
        
    
        public GameState(PlayerColor playerColor, BoardModel board)
        {
            Player = playerColor;

            CurrentTurn = PlayerColor.White;

            _computerPlayer = new ComputerPlayer(1);

            Board = board;

            _stateString = new FENString(CurrentTurn, Board).ToString();
            
            _stateHistory = new Stack<string>();

            _stateHistory.Push(_stateString);

            Result = null;

            _boardHystory = new Stack<BoardModel>();

            CapchuredPieces = new Stack<IPiece>();

        }

        public void ResetBoard() => 
            Board = BoardModel.InitialBoard(_stateString);

        private PlayerColor ChangeTurn() =>
            CurrentTurn == PlayerColor.White ? PlayerColor.Black: PlayerColor.White;

        public List<Position> MakeMove(IMove move)
        {
            Board.SetPawnSkipedPosition(CurrentTurn, null);

            _boardHystory.Push(Board.Copy());

            if (!Board.IsEmptySquare(move.To))
            {
                Board.CapturadePiece(Board[move.To]);
                CapchuredPieces.Push(Board[move.To]);
            }
            
            var moveToPositions = move.MoveTo(Board);
            
            CurrentTurn = ChangeTurn();
          
            UpdateStateString();
           
            return moveToPositions;
        }

        public void UndoMove(IMove move)
        {
            CurrentTurn = ChangeTurn();
            
            _stateHistory.Pop();
            
            if (_boardHystory.Count > 0)
                Board = _boardHystory.Pop();

        }

        public IMove MakeComputerMove() =>
            _computerPlayer.GetBestMove(this);
        

        public void CheckGameOver() 
        {
            if (!Board.AllLegalPlayerMoves(CurrentTurn).Any())
            {
                if (Board.IsInCheck(CurrentTurn))
                    Result = new GameResult(ChangeTurn(), EndGameType.Checkmate);
                else
                    Result = new GameResult(EndGameType.Stalemate);
            }

            else if (Board.InsufficientMaterial())
                Result = new GameResult(EndGameType.InsufficientMaterial);
            

        }

        public bool IsGameOver() =>
             Result is not null;
        

        private void UpdateStateString()
        {
            _stateString = new FENString(CurrentTurn, Board).ToString();
            _stateHistory.Push(_stateString);
        }
    }
}
