using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessOpeningsWPF.Chess.SortOfChessAI
{
    public class ComputerPlayer
    {
        public IMove bestMove = null;
        public static int Depth { get; private set; }
        public static int Infitity => 100000;

        private PlayerColor _currentTurn;

        public ComputerPlayer(int depth, PlayerColor currentTurn)
        {
            Depth = depth;
            _currentTurn = currentTurn;
        }

        private PlayerColor ChangeTurn() =>
                    _currentTurn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        public int SerchBestMove(int alpha, int beta, int depth, GameState state, BoardModel board)
        {
            if(depth == 0)
                return 0;

            var moves = state.AllLegalPlayerMoves(_currentTurn);
            if (moves.Count == 0) 
                return 0;

            int evaluation = 0;
            foreach (var move in moves)
            {
                move.MoveTo(board);
                _currentTurn = ChangeTurn();

                evaluation = SerchBestMove(alpha, beta, depth - 1, state, board);

                    if (evaluation < beta)
                    {
                        bestMove = move;
                        beta = evaluation;
                        return beta;
                    }
                    if (evaluation > alpha)
                    {
                        bestMove = move;
                        alpha = evaluation;

                        return alpha;
                    }
                }
            return alpha;
        }  
        
        
    }
}
