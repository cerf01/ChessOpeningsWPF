using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board.Movement;
using System.Collections.Generic;

namespace ChessOpeningsWPF.Chess.Openings
{
    public static class ChessOpeningsList
    {
        private static ChessOpening _kingsFianchettoOpening = new ChessOpening(
                "King's Fianchetto Opening",
                new List<IMove>()
                {
                    new NormalMove(new Position(6, 6), new Position(5,6))
                });

        private static ChessOpening _kingsPawnOpening = new ChessOpening(
                 "King's Pawn Opening",
                 new List<IMove>()
                 {
                    new NormalMove(new Position(6, 4), new Position(4,4))
                 });

        private static ChessOpening _queensPawnOpening = new ChessOpening(
                "Queen's Pawn Opening",
                new List<IMove>()
                {
                    new NormalMove(new Position(6, 3), new Position(4,3))
                });

        private static ChessOpening _nimzowitschLarsenAttack = new ChessOpening(
                "Nimzowitsch-Larsen Attack",
                new List<IMove>()
                {
                    new NormalMove(new Position(6, 1), new Position(5,1))
                });

        private static ChessOpening _englishOpening = new ChessOpening(
                 "English Opening",
                 new List<IMove>()
                 {
                    new NormalMove(new Position(6, 2), new Position(4,2))
                 });

        private static ChessOpening _birdsOpening = new ChessOpening(
                "Bird's Opening",
                new List<IMove>()
                {
                    new NormalMove(new Position(6, 5), new Position(4,5))
                });

        private static ChessOpening _retiOpening  = new ChessOpening(
                "Réti Opening",
                new List<IMove>()
                {
                    new NormalMove(new Position(7, 6), new Position(5,4))
                });

        private static ChessOpening _sicilianDefense = new ChessOpening(
               "Sicilian Defense",
               new List<IMove>()
               {
                    _kingsPawnOpening.Moves[0],
                    new NormalMove(new Position(1,2), new Position(3,2))
               });

        private static ChessOpening _frenchDefense = new ChessOpening(
              "French Defense",
              new List<IMove>()
              {
                    _kingsPawnOpening.Moves[0],
                    new NormalMove(new Position(1,6), new Position(2,6)),
              });

        private static ChessOpening _ruyLopezOpening = new ChessOpening(
             "Ruy López Opening",
             new List<IMove>()
             {
                   _frenchDefense.Moves[0],
                    new NormalMove(new Position(1,4), new Position(3,4)),
                   new NormalMove(new Position(7,6), new Position(5,5)),
                   new NormalMove(new Position(0,1), new Position(2,2)),
                   new NormalMove(new Position(7,5), new Position(3,1)),
             });

        private static ChessOpening _caroKannDefense = new ChessOpening(
             "Caro-Kann Defense",
             new List<IMove>()
             {
                  _frenchDefense.Moves[0],
                  new NormalMove(new Position(1,2), new Position(2,2)),
             });

        private static ChessOpening _italianGame = new ChessOpening(
           "Italian Game",
           new List<IMove>()
           {
                   _ruyLopezOpening.Moves[0],
                   _ruyLopezOpening.Moves[1],
                   _ruyLopezOpening.Moves[2],
                   _ruyLopezOpening.Moves[3],
                   new NormalMove(new Position(7,5), new Position(4,2)),
           });

        private static ChessOpening _scandinavianDefense = new ChessOpening(
            "Scandinavian Defense",
            new List<IMove>()
            {
                _frenchDefense.Moves[0],
                 new NormalMove(new Position(1,3), new Position(3,3)),
            });
        private static ChessOpening _pircDefense = new ChessOpening(
            "Pirc Defense",
            new List<IMove>()
            {
                _frenchDefense.Moves[0],
                 new NormalMove(new Position(1,3), new Position(2,3)),
                 _queensPawnOpening.Moves[0],
                 new NormalMove(new Position(0,6), new Position(2,5)),
            });

        private static ChessOpening _alekhinesDefense = new ChessOpening(
           "Alekhine's Defense",
           new List<IMove>()
           {
                _frenchDefense.Moves[0],
                _pircDefense.Moves[3],
           });

        private static ChessOpening _kingsGambit = new ChessOpening(
            "King's Gambit",
            new List<IMove>()
            {
                 _frenchDefense.Moves[0],
                 new NormalMove(new Position(1,3), new Position(2,3)),
                 new NormalMove(new Position(6,5), new Position(4,5)),
            });

        private static ChessOpening _scotchGame = new ChessOpening(
           "Scotch Game",
           new List<IMove>()
           {
                 _ruyLopezOpening.Moves[0],
                 _ruyLopezOpening.Moves[1],
                 _ruyLopezOpening.Moves[2],
                 _ruyLopezOpening.Moves[3],
                 _queensPawnOpening.Moves[0],
           });

        private static ChessOpening _viennaGame = new ChessOpening(
           "Vienna Game",
           new List<IMove>()
           {
                _ruyLopezOpening.Moves[0],
                _ruyLopezOpening.Moves[1],
                new NormalMove(new Position(7,1), new Position(5,2)),
           });

        private static ChessOpening _queensGambit  = new ChessOpening(
           "Queen's Gambit ",
           new List<IMove>()
           {
                _queensPawnOpening.Moves[0],
                _scandinavianDefense.Moves[1],
                 new NormalMove(new Position(6,2), new Position(4,2)),
           });

        private static ChessOpening _slavDefense = new ChessOpening(
           "Slav Defense",
           new List<IMove>()
           {
                _queensGambit.Moves[0],
                _queensGambit.Moves[1],
                _queensGambit.Moves[2],
               _caroKannDefense.Moves[1],
           });
        private static ChessOpening _kingsIndianDefense  = new ChessOpening(
          "King's Indian Defense",
          new List<IMove>()
          {
                _queensGambit.Moves[0],
                _pircDefense.Moves[3],
                _queensGambit.Moves[2],
               new NormalMove(new Position(1,6), new Position(2,6)),
          });

        private static ChessOpening _trompowskyAttack = new ChessOpening(
        "Trompowsky Attack",
        new List<IMove>()
        {
                _queensGambit.Moves[0],
                _pircDefense.Moves[3],
               new NormalMove(new Position(7,2), new Position(3,6)),
        });
        private static ChessOpening _londonSystem  = new ChessOpening(
            "London System ",
            new List<IMove>()
            {
                _queensGambit.Moves[0],
                _slavDefense.Moves[1],
                _ruyLopezOpening.Moves[2],
                _pircDefense.Moves[3],
               new NormalMove(new Position(7,2), new Position(4,5)),
            });
        private static ChessOpening _nimzoIndianDefense = new ChessOpening(
            "Nimzo-Indian Defense",
            new List<IMove>()
            {
                _queensGambit.Moves[0],
                _kingsIndianDefense.Moves[1],
                _queensGambit.Moves[2],
                 new NormalMove(new Position(1,4), new Position(2,4)),
                
                _viennaGame.Moves[2],
                 new NormalMove(new Position(0,5), new Position(4,1)),

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
            _frenchDefense,
            _ruyLopezOpening,
            _caroKannDefense,
            _italianGame,
            _scandinavianDefense,
            _pircDefense,
            _alekhinesDefense,
            _kingsGambit,
            _scotchGame,
            _viennaGame,
            _queensGambit,
            _slavDefense,
            _kingsIndianDefense,
            _trompowskyAttack,
            _londonSystem,
            _nimzoIndianDefense,
        };

    }
}
