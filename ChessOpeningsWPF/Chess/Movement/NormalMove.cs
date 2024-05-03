﻿using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessOpeningsWPF.Chess.Movement
{
    public class NormalMove : IMove
    {
        public Position From {  get; }

        public Position To { get; }

        public MoveType Type { get; }


        public NormalMove(Position from, Position to)
        {
            Type = MoveType.Normal;
            From = from;
            To = to;
        }

        public List<Position> MoveTo(BoardModel board)
        {
            IPiece piece = board[From];
            
            if (piece is null)
                return null;

            board[To] = piece;
            board[From] = null;
            piece.HasMoved = true;

            return new List<Position>() { From, To };
        }
    }
}