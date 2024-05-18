using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Board.Movement;
using ChessOpeningsWPF.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ChessOpeningsWPF.Chess.Game.SortOfChessAI
{
    public class ComputerPlayer
    {
       public static int Depth { get; private set; }

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

        public static int[,] Invert(int[,] table)
        {
            int[,] ret = new int[8, 8];

            for (int i = 0; i <= 7; i++)
                for (int j = 0; j <= 7; j++)
                    ret[i, j] = table[(7 - i), j];

            return ret;
        }

        public IMove GetBestMove(GameState state)
        {
            var possibleMoves = state.Board.AllLegalPlayerMoves(state.CurrentTurn);

            SerchBestMove(state, Depth, int.MinValue, int.MaxValue); 

            OrderMoves(possibleMoves, state);

            var bestMove = possibleMoves.First();

            return bestMove;
        }

        int SerchBestMove(GameState state, int depth, int alpha, int beta)
        {
            if (depth == 0)
                return SearchAllCapture(state, alpha, beta);

            var moves = state.Board.AllLegalPlayerMoves(state.CurrentTurn);

            if (moves.Count == 0)
            {
                if (state.Board.IsInCheck(state.CurrentTurn))
                    return int.MinValue;

                return 0;
            }
            OrderMoves(moves, state);
            foreach (var move in moves)
            {
                state.MakeMove(move);
               
                int evaluation = -SerchBestMove(state, depth - 1, -beta, -alpha);
               
                state.UndoMove(move);

                if (evaluation >= beta)
                    return beta;
                
                alpha = Math.Max(alpha, evaluation);
            }
            return alpha;
        }

        bool MoveIsCheck(GameState state, IMove move)
        {
            state.MakeMove(move);
            
            bool isMate = state.Board.IsInCheck(state.CurrentTurn);
            
            state.UndoMove(move);
            
            return isMate;
        }

        bool PositionOnAttack(BoardModel board, PlayerColor color, Position position) =>
          board.IsInAttack(color, position);

        bool MoveIsOnAttack(GameState state, IMove move)
        {            
            state.MakeMove(move);
            
            var onAttack = PositionOnAttack(state.Board, state.CurrentTurn, move.To);
            
            state.UndoMove(move);
            
            return onAttack;
        }

        public int SearchAllCapture(GameState state, int alpha, int beta)
        {
            int evaluation = CalculatePoint(state);
           
            if (evaluation >= beta) 
                return beta;
           
            alpha = Math.Max(alpha, evaluation);

            var captureMoves = state.Board.AllCapturePlayerMoves(state.CurrentTurn);
           
            OrderMoves(captureMoves, state);
           
            foreach (var move in captureMoves)
            {
                state.MakeMove(move);
                
                evaluation = -SearchAllCapture(state, -beta, -alpha);
               
                state.UndoMove(move);

                if (evaluation >= beta)
                    return beta;

                alpha = Math.Max(alpha, evaluation);
            }
            return alpha;
        }

        private int CalculatePoint(GameState state)
        {
            var whitePieces = 0;
            var blackPieces = 0;
           
            state.Board.PiecePositionsByColor(PlayerColor.White).ForEach(p => whitePieces += state.Board[p].Value);
            state.Board.PiecePositionsByColor(PlayerColor.Black).ForEach(p => whitePieces += state.Board[p].Value);

            var perspective = state.CurrentTurn == PlayerColor.White ? 1 : -1;

            return (whitePieces - blackPieces) * perspective;
        }
    

        private int GetBestPosition(IPiece piece, Position square)
        {
            var source = piece.Color == PlayerColor.Black ? _bestBPos : _bestWPos;

            switch (piece.Type)
            {
                case PieceType.Pawn:
                    return source[0][square.Row, square.Column];
                case PieceType.Night:
                    return source[1][square.Row, square.Column];
                case PieceType.Bishop:
                    return source[2][square.Row, square.Column];
                case PieceType.Rook:
                    return source[3][square.Row, square.Column];
                case PieceType.Queen:
                    return source[4][square.Row, square.Column];
                case PieceType.King:
                    return source[5][square.Row, square.Column];
            }
            return 0;
        }

        private void OrderMoves(List<IMove> moves, GameState state)
        {   
            int[] moveScore = new int[moves.Count];

            for (int i = 0; i < moves.Count; i++)
            {
                var movedPiece = state.Board[moves[i].From];

                moveScore[i] = 0;
                
                if(!state.Board.IsEmptySquare(moves[i].To))
                   moveScore[i] = 10 * state.Board[moves[i].To].Value - movedPiece.Value;

                 if (MoveIsCheck(state, moves[i]))
                {
                    if (MoveIsOnAttack(state, moves[i]))
                        moveScore[i] += movedPiece.Value;
                    else
                        moveScore[i] += int.MaxValue;
                }
                if (PositionOnAttack(state.Board, movedPiece.Color, moves[i].From))
                    moveScore[i] -= movedPiece.Value;

                if (MoveIsOnAttack(state, moves[i]))
                    moveScore[i] -= movedPiece.Value * 2;

                moveScore[i] += GetBestPosition(movedPiece, moves[i].To);

                if (moves[i].Type == MoveType.PawnPromotion)
                    moveScore[i] += new Queen(PlayerColor.None, new Position(0,0)).Value;             

            }
            Quicksort(moves, moveScore, 0, moves.Count - 1);
        }

        public static void Quicksort(List<IMove> values, int[] scores, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(values, scores, low, high);
                Quicksort(values, scores, low, pivotIndex - 1);
                Quicksort(values, scores, pivotIndex + 1, high);
            }
        }

        static int Partition(List<IMove> values, int[] scores, int low, int high)
        {
            int pivotScore = scores[high];
            int i = low - 1;

            for (int j = low; j <= high - 1; j++)
            {
                if (scores[j] > pivotScore)
                {
                    i++;
                    (values[i], values[j]) = (values[j], values[i]);
                    (scores[i], scores[j]) = (scores[j], scores[i]);
                }
            }
            (values[i + 1], values[high]) = (values[high], values[i + 1]);
            (scores[i + 1], scores[high]) = (scores[high], scores[i + 1]);

            return i + 1;
        }

    }
}
