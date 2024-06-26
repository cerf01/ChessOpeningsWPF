﻿using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Movment;
using ChessOpeningsWPF.Chess.Pieces;

namespace ChessOpeningsWPF.Chess.Board
{
    public class BoardModel
    {
        private readonly IPiece[,] _pieces = new IPiece[8,8];


        public IPiece this[int row, int col] 
        {
            get { return _pieces[row,col]; }
            set { _pieces[row,col] = value; }
        }

        public IPiece this[Position position]
        {
            get { return _pieces[position.Row,position.Column]; }
            set { _pieces[position.Row,position.Column] = value; }
        }

        private void SetPieces()
        {
            this[0, 0] = new Rook(PieceColor.Black);
            this[0, 1] = new Knight(PieceColor.Black);
            this[0, 2] = new Bishop(PieceColor.Black);
            this[0, 3] = new Queen(PieceColor.Black);
            this[0, 4] = new King(PieceColor.Black);
            this[0, 5] = new Bishop(PieceColor.Black);
            this[0, 6] = new Knight(PieceColor.Black);
            this[0, 7] = new Rook(PieceColor.Black);

           
            this[7, 0] = new Rook(PieceColor.White);
            this[7, 1] = new Knight(PieceColor.White);
            this[7, 2] = new Bishop(PieceColor.White);
            this[7, 3] = new Queen(PieceColor.White);
            this[7, 4] = new King(PieceColor.White);
            this[7, 5] = new Bishop(PieceColor.White);
            this[7, 6] = new Knight(PieceColor.White);
            this[7, 7] = new Rook(PieceColor.White);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(PieceColor.Black);
                this[6, c] = new Pawn(PieceColor.White);
            }

        }

        public static BoardModel InitialBoard()
        {
            BoardModel board = new BoardModel();
            board.SetPieces();
            return board;
        }
    }
}
