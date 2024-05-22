using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System.Text;

namespace ChessOpeningsWPF.Chess.Game
{
    public class FENString
    {
        private StringBuilder _stringBuilder = new StringBuilder();
        public FENString(PlayerColor currentPlayer, BoardModel board)
        {
            AddPiecePlacment(board);
            _stringBuilder.Append(' ');

            _stringBuilder.Append(char.ToLower(currentPlayer.ToString()[0]));
            _stringBuilder.Append(' ');

            AddCatlingsRights(board);
            _stringBuilder.Append(' ');

            AddEnPassant(currentPlayer, board);

        }

        private static char PieceChar(IPiece piece) =>
            piece.Color == PlayerColor.White ? piece.Type.ToString()[0] :  char.ToLower(piece.Type.ToString()[0]);

        private void AddRowData(BoardModel board, int row)
        {
            int empty = 0;

            for (int c = 0; c < 8; c++)
            {
                if (board[row, c] is null)
                    empty++;
                else
                {
                    if (empty > 0)
                    {
                        _stringBuilder.Append(empty);
                        empty = 0;
                    }
                    _stringBuilder.Append(PieceChar(board[row, c]));
                }
            }

            if (empty > 0)
            {
                _stringBuilder.Append(empty);
            }

        }

        private void AddPiecePlacment(BoardModel board)
        {
            for (int r = 0; r < 8; r++)
            {
                if (r > 0)
                    _stringBuilder.Append('/');
                AddRowData(board, r);
            }
        }

        private void AddCatlingsRights(BoardModel board)
        {
            var castlingWR = board.CanCaslteRightSide(PlayerColor.White);
            var castlingWL = board.CanCaslteLeftSide(PlayerColor.White);

            var castlingBR = board.CanCaslteRightSide(PlayerColor.Black);
            var castlingBL = board.CanCaslteLeftSide(PlayerColor.Black);

            if (!(castlingWR || castlingWL || castlingBR || castlingBL))
            {
                _stringBuilder.Append('-');
                return;
            }
            if (castlingWR)
                _stringBuilder.Append('r');

            if (castlingWL)
                _stringBuilder.Append('l');

            if (castlingBR)
                _stringBuilder.Append('R');

            if (castlingBL)
                _stringBuilder.Append('L');
        }

        private void AddEnPassant(PlayerColor color, BoardModel board)
        {
            if (!board.CanMakeEnPassant(color))
            {
                _stringBuilder.Append('-');
                return;
            }

            var position = board.GetPawnSkipedPositions(color);

            _stringBuilder.Append('a' + position.Column);
            _stringBuilder.Append(position.Row);
        }

        public override string ToString() =>
            _stringBuilder.ToString();

    }
}
