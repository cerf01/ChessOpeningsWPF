using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessOpeningsWPF.Chess.Abstractions.Interfaces
{
    public interface ILongStepPiece
    {
        public List<Position> MovePositions(Position currPosition, Direction direction, BoardModel board);

        public List<Position> MoveDirections(Position currPosition, List<Direction> directions, BoardModel board);

    }
}
