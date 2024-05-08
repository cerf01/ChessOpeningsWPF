using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using ChessOpeningsWPF.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ChessOpeningsWPF.Chess.SortOfChessAI
{
    public class ComputerPlayer
    {
        public IMove bestMove = null;
        public static int Depth { get; private set; }
        public static int Infitity => 100000;



       private static int[,] bestPawnPositions = new int[,]{
            {0,  0,  0,  0,  0,  0,  0,  0},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5, -5,-10,  0,  0,-10, -5,  5},
            {5, 10, 10,-20,-20, 10, 10,  5},
            {0,  0,  0,  0,  0,  0,  0,  0}};

        private static int[,] bestNightPositions = new int[,]{
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}};

        private static int[,] bestBishopPositions = new int[,]{
                { -20,-10,-10,-10,-10,-10,-10,-20},
                {-10,  0,  0,  0,  0,  0,  0,-10},
                {-10,  0,  5, 10, 10,  5,  0,-10},
                {-10,  5,  5, 10, 10,  5,  5,-10},
                {-10,  0, 10, 10, 10, 10,  0,-10},
                {-10, 10, 10, 10, 10, 10, 10,-10},
                {-10,  5,  0,  0,  0,  0,  5,-10},
                {-20,-10,-10,-10,-10,-10,-10,-20}};

        private static int[,] bestRookPositions = new int[,]{
                 { 0,  0,  0,  0,  0,  0,  0,  0},
                 { 5, 10, 10, 10, 10, 10, 10,  5},
                 {-5,  0,  0,  0,  0,  0,  0, -5},
                 {-5,  0,  0,  0,  0,  0,  0, -5},
                 {-5,  0,  0,  0,  0,  0,  0, -5},
                 {-5,  0,  0,  0,  0,  0,  0, -5},
                 {-5,  0,  0,  0,  0,  0,  0, -5},
                 { 0,  0,  0,  5,  5,  0,  0,  0}};

        private static int[,] bestQueenPositions = new int[,]{
                   { -20,-10,-10, -5, -5,-10,-10,-20},
                    {-10,  0,  0,  0,  0,  0,  0,-10},
                    {-10,  0,  5,  5,  5,  5,  0,-10},
                    { -5,  0,  5,  5,  5,  5,  0, -5},
                    {  0,  0,  5,  5,  5,  5,  0, -5},
                    {-10,  5,  5,  5,  5,  5,  0,-10},
                    {-10,  0,  5,  0,  0,  0,  0,-10},
                    {-20,-10,-10, -5, -5,-10,-10,-20}};


        private static int[,] bestKingPositions = new int[,]{
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            { 20, 20,  0,  0,  0,  0, 20, 20},
            { 20, 30, 10,  0,  0, 10, 30, 20}};


        private List<int[,]> _bestWPos = new List<int[,]>() 
        { 
            bestPawnPositions,
            bestNightPositions,
            bestBishopPositions,
            bestRookPositions,
            bestQueenPositions,
            bestKingPositions
        };

        private List<int[,]> _bestBPos = new List<int[,]>()        
        {
           Invert(bestPawnPositions),
            Invert(bestNightPositions),
            Invert(bestBishopPositions),
            Invert(bestRookPositions),
            Invert(bestQueenPositions),
            Invert(bestKingPositions)
        };


        public ComputerPlayer(int depth)
        {
            Depth = depth;

        }



        public int CalculatePoint(BoardModel board, bool isWhite)
        {
            int scoreWhite = 0;
            int scoreBlack = 0;

            board.PiecePositionsByColor(PlayerColor.White).ForEach(p => scoreWhite += board[p].Value);
            board.PiecePositionsByColor(PlayerColor.Black).ForEach(p => scoreBlack += board[p].Value);

            int evaluation = scoreWhite - scoreBlack;

            int prespective = (isWhite) ? 1 : -1;
            return evaluation * prespective;
        }

        private static int[,] Invert(int[,] table)
        {
            int[,] ret = new int[8, 8];
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    ret[i, j] = table[(7 - i), j];
                }
            }
            return ret;
        }

        public IMove GetbestMove(GameState state) 
        {
            int bestValue = int.MinValue;
            List<IMove> possibleMoves = state.Board.AllLegalPlayerMoves(state.CurrentTurn);

            OrderMoves(possibleMoves, state.Board);
            foreach (var move in possibleMoves)
            {
                var newBoard = state.Board.Copy();
                move.MoveTo(newBoard);
                int value = SerchBestMove(int.MinValue, int.MaxValue, Depth, newBoard, true) ;
                if (value >= bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }
          
            return bestMove;
        }

        public int SerchBestMove(int alpha, int beta, int depth, BoardModel board, bool isMaximizing)
        {
            if(depth == 0)
                return CalculatePoint(board, isMaximizing);

            if (isMaximizing)
            {
                int bestValue = int.MinValue;

                var moves = board.AllLegalPlayerMoves(PlayerColor.Black);

                int evaluation = 0;

                OrderMoves(moves, board);
                foreach (var move in moves)
                {
                    var temp = board[move.From];
                    var newBoard = board.Copy();
                    move.MoveTo(newBoard);

                    evaluation = SerchBestMove(alpha, beta, depth - 1, newBoard, false);

                    bestValue = Math.Max(bestValue, evaluation);

                    alpha = Math.Max(alpha, evaluation);

                    if (beta <= alpha)
                        return bestValue;

                }
                return bestValue;

            }
            else
            {
                int bestValue = int.MaxValue;

                var moves = board.AllLegalPlayerMoves(PlayerColor.White);

                int evaluation = 0;

                OrderMoves(moves, board);
                foreach (var move in moves)
                {
                    var temp = board[move.From];
                    var newBoard = board.Copy();
                    move.MoveTo(newBoard);

                    evaluation = SerchBestMove(alpha, beta, depth - 1, newBoard, true);

                    bestValue = Math.Min(bestValue, evaluation);

                    beta = Math.Min(beta, evaluation);

                    if (beta <= alpha)
                          return bestValue;

                }
                return bestValue;
            }

        }

        private int GetBestPosition(IPiece piece, Position position)
        {
            if (piece.Color == PlayerColor.White)
            {
                switch (piece.Type)
                {
                    case PieceType.Pawn:
                        return _bestWPos[0][position.Row, position.Column];
                    case PieceType.Night:
                        return _bestWPos[1][position.Row, position.Column];
                    case PieceType.Bishop:
                        return _bestWPos[2][position.Row, position.Column];
                    case PieceType.Rook:
                        return _bestWPos[3][position.Row, position.Column];
                    case PieceType.Queen:
                        return _bestWPos[4][position.Row, position.Column];
                    case PieceType.King:
                        return _bestWPos[5][position.Row, position.Column];   
                }
            }

            switch (piece.Type)
            {
                case PieceType.Pawn:
                    return _bestBPos[0][position.Row, position.Column];
                case PieceType.Night:
                    return _bestBPos[1][position.Row, position.Column];
                case PieceType.Bishop:
                    return _bestBPos[2][position.Row, position.Column];
                case PieceType.Rook:
                    return _bestBPos[3][position.Row, position.Column];
                case PieceType.Queen:
                    return _bestBPos[4][position.Row, position.Column];
                case PieceType.King:
                    return _bestBPos[5][position.Row, position.Column];
            }

            return 0;

        }

/*        private void OrderMoves(List<IMove> moveList, BoardModel board)
        {
            int[] moveScore = new int[moveList.Count];

            foreach (var move in moveList) 
            {
                var movePiece = board[move.From];
                var capturatePiece = board[move.To];

                if (board[move.To] != null)
                {                    
                    moveScore[moveList.IndexOf(move)] += 10 * capturatePiece.Value - movePiece.Value;

                }
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

                var bestMove = moveList[bestScoreIndex];
                moveList[bestScoreIndex] = moveList[sorted];
                moveList[sorted] = bestMove;
            }

        }*/

        private void OrderMoves(List<IMove> moveList, BoardModel board)
        {
            int[] moveScore = new int[moveList.Count];

            for (int i = 0; i < moveList.Count; i++)
            {
                moveScore[i] = 0;

                if (board[moveList[i].To] != null)
                {
                    moveScore[i] += 10 * board[moveList[i].To].Value - board[moveList[i].From].Value;
                }

                if (board.PawnPromoted(moveList[i].From))
                {
                    moveScore[i] += new Queen(PlayerColor.Black, new Position(0,0)).Value;
                }

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

                var bestMove = moveList[bestScoreIndex];
                moveList[bestScoreIndex] = moveList[sorted];
                moveList[sorted] = bestMove;
            }
        }

    }
}
