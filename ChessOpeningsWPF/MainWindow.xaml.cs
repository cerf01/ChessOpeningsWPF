using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movment;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.IO;
using System.Media;
using ChessOpeningsWPF.Chess.AssetsSource;
using System.Windows.Input;

namespace ChessOpeningsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<List<Image>> _piecesAssets = new List<List<Image>>();
        private BoardModel _board = new BoardModel();
        private SoundPlayer _soundPlayer = new SoundPlayer(Path.GetFullPath("../../../Chess/AssetsSource/SoundAssets/MovePieceSound.wav"));
        private bool _isUsed = false;
       
        public MainWindow()
        {
            InitializeComponent();
            InitialBoard();
            _board = BoardModel.InitialBoard();
            DrawPieces(_board);
            _soundPlayer.Load();
        }

        
        private void InitialBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                _piecesAssets.Add(new List<Image>());
                for (int c = 0; c < 8; c++)
                {
                    var image = new Image();
                    _piecesAssets[r].Add(image);
                    GameBoard.Children.Add(image);
                }
            }
        }

        private void DrawPiece(IPiece? piece, int r, int c)
        {
            if (piece is null)
            {
                _piecesAssets[r][c].Source = new BitmapImage();
                return;
            }
            _piecesAssets[r][c].Source = AssetsLoader.GetAsset(piece.Color, piece.Type);

        }

        public void HandelMove(IMove move)
        {
            var fromToPositions = move.MoveTo(_board);

            if (fromToPositions is null)
                return;

            foreach (var position in fromToPositions)
                DrawPiece(_board[position.Row, position.Column], position.Row, position.Column);
        }

        public async Task HandelMoves(List<IMove> moves)
        {
           
            if (_isUsed)
            {
                _board = BoardModel.InitialBoard();
                DrawPieces(_board);
            }
            await Task.Delay(500);
            foreach (var move in moves)
            {
                HandelMove(move);
               
                _soundPlayer.Play();
                await Task.Delay(1000);
            }
            _isUsed = true;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            List<IMove> moves = null;

            switch (btn.Name)
            {
                case "opennding1":
                    {
                        moves = new List<IMove>()
                        {
                             new MovePiece(new Position(6, 6), new Position(5,6))
                        };
                    }
                    break;
                case "opennding2":
                    {
                        moves = new List<IMove>()
                        {
                              new MovePiece(new Position(6, 3), new Position(4,3))
                        };                   
                    }
                    break;

                case "opennding3":
                    {
                        moves = new List<IMove>()
                        {
                              new MovePiece(new Position(6, 4), new Position(4,4))
                        };
                    }
                    break;

                case "opennding4":
                    {
                        moves = new List<IMove>()
                        {
                            new MovePiece(new Position(6, 1), new Position(5,1))
                        };
                    }
                    break;
                default:
                    {
                        moves = new List<IMove>()
                        {
                            new MovePiece(new Position(0, 0), new Position(0,0))
                        };
                    }break;
            }
            
            await HandelMoves(moves);

        }
    }
}