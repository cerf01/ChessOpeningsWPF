using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Board.Movement;
using System.Collections.Generic;



namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface IPiece
    {
        PieceType Type { get; }
        PlayerColor Color { get; }
        Position Position { get; set; }
        int Value { get; }
        bool HasMoved { get; set; }
      
        public List<Direction> Directions { get; }
        
        public IPiece Copy();           
       
        public List<IMove> GetMoves(Position currPosition, BoardModel board);

        public bool CanCaptureEnemyKing(Position position, BoardModel board);

        public bool CanCaptureEnemy(Position position, BoardModel board);
    }
}
