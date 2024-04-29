using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Movment;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Openings
{
    public static class ChessOpeningsList
    {
        private static ChessOpening _kingsFianchettoOpening = new ChessOpening(
                "King's Fianchetto Opening",
                new List<IMove>()
                {
                    new MovePiece(new Position(6, 6), new Position(5,6))
                });

        private static ChessOpening _kingsPawnOpening = new ChessOpening(
                 "King's Pawn Opening",
                 new List<IMove>()
                 {
                    new MovePiece(new Position(6, 4), new Position(4,4))
                 });

        private static ChessOpening _queensPawnOpening = new ChessOpening(
                "Queen's Pawn Opening",
                new List<IMove>()
                {
                    new MovePiece(new Position(6, 3), new Position(4,3))
                });

        private static ChessOpening _nimzowitschLarsenAttack = new ChessOpening(
                "Nimzowitsch-Larsen Attack",
                new List<IMove>()
                {
                    new MovePiece(new Position(6, 1), new Position(5,1))
                });

        private static ChessOpening _englishOpening = new ChessOpening(
                 "English Opening",
                 new List<IMove>()
                 {
                    new MovePiece(new Position(6, 2), new Position(4,2))
                 });

        private static ChessOpening _birdsOpening = new ChessOpening(
                "Bird's Opening",
                new List<IMove>()
                {
                    new MovePiece(new Position(6, 5), new Position(4,5))
                });

        private static ChessOpening _retiOpening  = new ChessOpening(
                "Réti Opening",
                new List<IMove>()
                {
                    new MovePiece(new Position(7, 6), new Position(5,4))
                });

        private static ChessOpening _sicilianDefense = new ChessOpening(
               "Sicilian Defense",
               new List<IMove>()
               {
                    _kingsPawnOpening.Moves[0],
                    new MovePiece(new Position(1, 2), new Position(3,2))
               });

        public static List<ChessOpening> ChessOpenings = new List<ChessOpening>()
        {
            _kingsPawnOpening,
            _queensPawnOpening,
            _kingsFianchettoOpening,
            _nimzowitschLarsenAttack,
            _englishOpening,
            _birdsOpening,
            _retiOpening,
            _sicilianDefense,

        };

    }
}
