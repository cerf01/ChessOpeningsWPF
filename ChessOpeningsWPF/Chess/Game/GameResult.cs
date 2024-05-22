using ChessOpeningsWPF.Chess.Abstractions.Enums;

namespace ChessOpeningsWPF.Chess
{
    public class GameResult
    {
        public PlayerColor Winner { get;}
        public EndGameType Reason { get;}

        public GameResult(EndGameType reason)
        {
            Winner = PlayerColor.None;
            Reason = reason;
        }

        public GameResult(PlayerColor winner, EndGameType reason)
        {
            Winner = winner;
            Reason = reason;
        }

        
    }
}
