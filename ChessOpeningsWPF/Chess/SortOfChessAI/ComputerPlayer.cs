using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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



        public int CalculatePoint(BoardModel board)
        {
            int scoreWhite = 0;
            int scoreBlack = 0;

            board.PiecePositionsByColor(PlayerColor.White).ForEach(p => scoreWhite += board[p].Value);
            board.PiecePositionsByColor(PlayerColor.Black).ForEach(p => scoreBlack += board[p].Value);

            int evaluation = scoreBlack - scoreWhite;

            int prespective = (_currentTurn == PlayerColor.Black) ? -1 : 1;
            return evaluation * prespective;
        }

        private void OrderMoves(List<IMove> moveList, BoardModel board)
        {
            int[] moveScore = new int[moveList.Count];

            for (int i = 0; i < moveList.Count; i++)
            {
                moveScore[i] = 0;

                if (moveList[i].To is null)
                    moveScore[i] += 10 * board[moveList[i].To].Value - board[moveList[i].From].Value;

            }

            for (int sorted = 0; sorted < moveList.Count; sorted++)
            {
                int bestScore = int.MinValue;
                int bestScoreIndex = 0;

                for (int i = sorted; i < moveList.Count; i++)
                {
                    if (moveScore[i] > bestScore)
                    {
                        bestScore = moveScore[i];
                        bestScoreIndex = i;
                    }
                }

                // swap

                IMove bestMove = moveList[bestScoreIndex];
                moveList[bestScoreIndex] = moveList[sorted];
                moveList[sorted] = bestMove;
            }
        }

        public int SerchBestMove(int alpha, int beta, int depth, GameState state, BoardModel board)
        {
            if(depth == 0)
                return CalculatePoint(board);

            var moves = state.AllLegalPlayerMoves(_currentTurn);
            if (moves.Count == 0) 
                return 0;


            int evaluation = 0;
            OrderMoves(moves, board);
            foreach (var move in moves)
            {
                move.MoveTo(board);
                evaluation = -SerchBestMove(-alpha, -beta, depth - 1, state, board);

                if (evaluation >= beta)
                    return beta;            
                bestMove = move;
                alpha = Math.Max(alpha, evaluation);
            }
            return alpha;
        }
    }
}
