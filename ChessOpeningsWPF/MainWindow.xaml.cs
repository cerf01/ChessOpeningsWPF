using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.IO;
using System.Media;
using ChessOpeningsWPF.Chess.AssetsSource;
using ChessOpeningsWPF.Chess.Openings;
using ChessOpeningsWPF.Controls;

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
           
            BoardGrid.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath("../../../Chess/AssetsSource/Assets/Board/BoardBackground.png"), UriKind.Absolute)));
           
            InitButtons();

            InitialBoard();

            _board = BoardModel.InitialBoard();
           
            DrawPieces(_board);

            _soundPlayer.Load();
        }

        private void InitButtons() 
        {
            foreach (var opening in ChessOpeningsList.ChessOpenings)
            {
                var button = new OpeningButton(opening);
                button.OnClick += Button_OnClick;
                OpeningsButtons.Children.Add(button);
            }
        }

        private async Task Button_OnClick(List<IMove> moves)
        {
            OpeningsButtons.IsEnabled = false;

            await HandelMoves(moves);

            OpeningsButtons.IsEnabled = true;
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

        private void TryToReload() 
        {
            if (_isUsed)
            {
                _board = BoardModel.InitialBoard();
                DrawPieces(_board);
            }
            else
                _isUsed = true;
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
            TryToReload();

            await Task.Delay(500);    
            
            foreach (var move in moves)
            {
                HandelMove(move);

                _soundPlayer.Play();
                await Task.Delay(1000);
            }     
            
        }
    }
}