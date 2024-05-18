using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Media;
using ChessOpeningsWPF.Chess.AssetsSource;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using ChessOpeningsWPF.Chess.Abstractions.Enums;
using System.Linq;
using ChessOpeningsWPF.Chess.Openings;
using ChessOpeningsWPF.Controls;
using ChessOpeningsWPF.Chess.Board.Movement;
using ChessOpeningsWPF.Chess.Game;


namespace ChessOpeningsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public delegate void OnPositionSelect(Position position);

    public delegate void OnMove(SoundPlayer sound);



    public partial class MainWindow : Window
    {
        private List<List<Image>> _piecesAssets;

        private List<List<Rectangle>> _highlightsRectangles;

        private Dictionary<Position, IMove> _movesCache;

        private GameState _gameState;

        private SoundPlayer _soundPlayer;

        private Position _selectedPosition;

        private bool _isUsed;

        private event OnPositionSelect _onFromPositionSelect;

        private event OnPositionSelect _onToPositionSelect;

        private event OnMove _onMove;

        private Brush _squareToMove;
        private Brush _squareOnAttack;

        private string _stardedFEN => "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq";

        public MainWindow()
        {

            InitializeComponent();

            _piecesAssets = new List<List<Image>>();

            _highlightsRectangles = new List<List<Rectangle>>();

            _movesCache = new Dictionary<Position, IMove>();

            _soundPlayer = new SoundPlayer(System.IO.Path.GetFullPath("../../../Chess/Source/Sounds/MovePieceSound.wav"));

            _isUsed = false;

            InitialBoard();

            InitButtons();

            _gameState = new GameState(PlayerColor.White, BoardModel.InitialBoard(_stardedFEN));

            DrawPieces(_gameState.Board);

            _soundPlayer.Load();

            _onFromPositionSelect += OnFromPositionSelect;

            _onToPositionSelect += OnToPositionSelect;

            _onMove += OnMove;

            GameState.MovePiece += GameState_MovePiece;

            _squareToMove = new SolidColorBrush(Color.FromArgb(185, 235, 195, 31));
            
            _squareOnAttack = new SolidColorBrush(Color.FromArgb(185, 175, 16, 16));

        }

        private void GameState_MovePiece(IMove move)
        {
            DrawPiece(_gameState.Board[move.From], move.From.Row, move.From.Column);
            DrawPiece(_gameState.Board[move.To], move.To.Row, move.To.Column);
        }

        private void OnMove(SoundPlayer sound)
        {
            sound.Play();
        }

        private void OnFromPositionSelect(Position position)
        {
            if (_gameState.Board[position] is null || _gameState.Board[position].Color != _gameState.Player)
                return;

            var moves = _gameState.Board.AvailableMovesForPiece(position, _gameState.CurrentTurn);

            if (moves.Any())
            {
                _selectedPosition = position;
                SetMovesCache(moves);
                ShowHighlights();
            }
            
        }

        private void OnToPositionSelect(Position position)
        {
            _selectedPosition = null;
            HideHighlights();

            if (_movesCache.TryGetValue(position, out IMove move))
                HandelMove(move);
        }

        private void InitButtons()
        {
            foreach (var opening in ChessOpeningsList.ChessOpenings)
            {
                var button = new OpeningButton(opening);
                button.OnClick += Button_OnClick;

                if (ChessOpeningsList.ChessOpenings.IndexOf(opening) % 2 == 0)
                {
                    button.ContentBorder.Background = new SolidColorBrush(Color.FromRgb(0xf7, 0xd1, 0x9e));
                    button.Title.Foreground = new SolidColorBrush(Color.FromRgb(0x76, 0x26, 0x02));
                }
                else
                {
                    button.ContentBorder.Background = new SolidColorBrush(Color.FromRgb(0x76, 0x26, 0x02));
                    button.Title.Foreground = new SolidColorBrush(Color.FromRgb(0xf7, 0xd1, 0x9e));
                }

                OpeningsButtons.Children.Add(button);  
            }
        }

        private async Task Button_OnClick(List<IMove> moves)
        {
            OpeningsButtons.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;

            await HandelMoves(moves);

            Mouse.OverrideCursor = Cursors.Arrow;
            OpeningsButtons.IsEnabled = true;
        }

        private void InitialBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                _piecesAssets.Add(new List<Image>());
                _highlightsRectangles.Add(new List<Rectangle>());
                for (int c = 0; c < 8; c++)
                {
                    var image = new Image();

                    _piecesAssets[r].Add(image);

                    GameBoard.Children.Add(image);

                    var rectangle = new Rectangle();
                   
                    _highlightsRectangles[r].Add(rectangle);

                    Highlight.Children.Add(rectangle);
                }
            }
        }

        private void DrawPiece(IPiece? piece, int r, int c) =>
            _piecesAssets[r][c].Source = piece is null ? new BitmapImage() : AssetsLoader.GetAsset(piece.Color, piece.Type);

        public void HandelMove(IMove move)
        {

            var moveToPositions = _gameState.MakeMove(move);

            if (moveToPositions is null)
                return;

            foreach (var position in moveToPositions)
                DrawPiece(_gameState.Board[position.Row, position.Column], position.Row, position.Column);
           
            _gameState.CheckGameOver();
            
            _onMove.Invoke(_soundPlayer);
        }

        private void DrawPieces(BoardModel board)
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    DrawPiece(board[r, c], r, c);           
        }

        private void SetMovesCache(List<IMove> moves)
        {
            _movesCache.Clear();

            foreach (var move in moves)
                _movesCache[move.To] = move;
        }

        private void ShowHighlights()
        {
            foreach (var position in _movesCache.Keys)
            {
                if (!_gameState.Board.IsEmptySquare(position) && _gameState.Board[position].Color != _gameState.CurrentTurn)
                    _highlightsRectangles[position.Row][position.Column].Fill = _squareOnAttack;
                else
                    _highlightsRectangles[position.Row][position.Column].Fill = _squareToMove;
            }
        }

        private void HideHighlights()
        {
            foreach (var position in _movesCache.Keys)
                _highlightsRectangles[position.Row][position.Column].Fill = Brushes.Transparent;
        }

        private Position GetSquarePosition(Point point)
        {
            double squareSize = BoardSquare.ActualWidth / 8;

            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);

            return new Position(row, col);
        }

        private async void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BoardSquare);

            var position = GetSquarePosition(point);

            if (_selectedPosition is null)
                _onFromPositionSelect.Invoke(position);
            else
                _onToPositionSelect.Invoke(position);

            await Task.Delay(250);

            if (_gameState.CurrentTurn != _gameState.Player)
            {

                HandelMove(_gameState.MakeComputerMove());
            }
        }

        /*                private void TryToReload()
                        {
                            if (_isUsed)
                            {
                                _gameState.Board =  BoardModel.InitialBoard();
                                DrawPieces(_board);
                            }
                            else
                                _isUsed = true;
                        }*/


        public async Task HandelMoves(List<IMove> moves)
        {
            /*  TryToReload();*/

            await Task.Delay(500);

            foreach (var move in moves)
            {
                await Task.Delay(500);
                HandelMove(move);
            }

        }

    }
} 
