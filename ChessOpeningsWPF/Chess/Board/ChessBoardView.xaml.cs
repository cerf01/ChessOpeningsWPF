using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board.AssetsSource;
using ChessOpeningsWPF.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessOpeningsWPF.Chess.Board
{
    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoard : UserControl
    {
        private Image[,] _piecesAssets = new Image[8,8];

        private BoardModel _board = new BoardModel();
        public ChessBoard()
        {
            InitializeComponent();
            InitialBoard();
            _board = BoardModel.InitialBoard();
            DrawPieces(_board); 

        }

        private void InitialBoard() 
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                   var image = new Image();
                    _piecesAssets[r, c] = image;
                    GameBoard.Children.Add(image);
                }
            }
        }

        private void DrawPiece(IPiece? piece, int r, int c)
        {
            if (piece is null)
            {
                _piecesAssets[r, c].Source = new BitmapImage();
                return;
            }
            _piecesAssets[r, c].Source = AssetsLoader.GetAsset(piece.Color, piece.Type); ;
            
        }

        public void HandelMove(IMove move) 
        {
            move.MoveTo(_board);
            DrawPieces(_board);
        }

        private void DrawPieces(BoardModel board) 
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    DrawPiece(board[r, c], r, c);
                }
            }
        }
    }
}
