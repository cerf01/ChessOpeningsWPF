using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Openings
{
    public class ChessOpening
    {
        public string Name { get;private set; }
        public List<IMove> Moves { get; private set; }

        public ChessOpening(string name, List<IMove> moves) 
        {
            Name = name;
            Moves = moves;
        }
    }
}
