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
using System.Windows.Documents;
using ChessOpeningsWPF.Windows;


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

        private bool _isSarted;

        private event OnPositionSelect _onFromPositionSelect;

        private event OnPositionSelect _onToPositionSelect;

        private event OnMove _onMove;

        private Brush _squareToMove;
        private Brush _squareOnAttack;
        private string _stardedFEN => "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -";
        private string _testEndFEN => "4k2r/6r1/8/8/8/8/3R4/R3K3 w Qk -";
        private string _soundPath => "../../../Chess/Source/Sounds/MovePieceSound.wav";

        private int moveCounr = 0;

        public MainWindow()
        {

            InitializeComponent();

            _piecesAssets = new List<List<Image>>();

            _highlightsRectangles = new List<List<Rectangle>>();

            _movesCache = new Dictionary<Position, IMove>();

            _soundPlayer = new SoundPlayer(System.IO.Path.GetFullPath(_soundPath));

            _isUsed = false;

            _isSarted = false;

            InitialBoard();

            InitButtons();

            _gameState = new GameState(PlayerColor.White, BoardModel.InitialBoard(_stardedFEN));

            DrawPieces(_gameState.Board);

            _soundPlayer.Load();

            _onFromPositionSelect += OnFromPositionSelect;

            _onToPositionSelect += OnToPositionSelect;

            _onMove += OnMove;

            _squareToMove = new SolidColorBrush(Color.FromArgb(185, 235, 195, 31));

            _squareOnAttack = new SolidColorBrush(Color.FromArgb(185, 175, 16, 16));

            

        }

        private void OnMove(SoundPlayer sound)
        {
            sound.Play();

            _gameState.CheckGameOver();

            if (_gameState.IsGameOver())
                ShowEndWindow();
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
            Cursor = Cursors.Wait;

            await HandelMoves(moves);

            Cursor = Cursors.Arrow;
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

            _onMove.Invoke(_soundPlayer);
            moveCounr++;

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
                if (!_gameState.Board.IsEmptySquare(position) && _gameState.Board[position].Color != _gameState.CurrentTurn )
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
            if (!_isSarted)
                return;
            Point point = e.GetPosition(BoardSquare);

            var position = GetSquarePosition(point);

            if (_selectedPosition is null)
                _onFromPositionSelect.Invoke(position);
            else
                _onToPositionSelect.Invoke(position);

            await Task.Delay(250);

            if (_gameState.CurrentTurn != _gameState.Player)
            {
                if (!_gameState.IsGameOver())
                {
                    var move = _gameState.MakeComputerMove();
                    HandelMove(move);
                }
            }
        }

        private void TryToReload()
        {
            if (_isUsed)
            {
                Restart();
            }
            else
                _isUsed = true;
        }

        public async Task HandelMoves(List<IMove> moves)
        {
            TryToReload();

            await Task.Delay(500);

            foreach (var move in moves)
            {
                await Task.Delay(500);

                HandelMove(move);
            }
        }

        private void Restart()
        {
            _isSarted = false;

            HideHighlights();

            _movesCache.Clear();

            _gameState = new GameState(PlayerColor.White, BoardModel.InitialBoard(_stardedFEN));

            DrawPieces(_gameState.Board);
        }

        private void ShowEndWindow()
        {
            var endGameWindow = new EndGameWindow(this, _gameState.Result);
            endGameWindow.ShowDialog();

            var restart = endGameWindow.TryAgain;
            if (restart)
                Restart();
            else
                Application.Current.Shutdown();
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if (_isSarted)
                return;

            _isSarted = true;
            if (_gameState.CurrentTurn != _gameState.Player)
            {
                var move = _gameState.MakeComputerMove();
                HandelMove(move);
            }
        }

        private void Btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        private void Btn_ToEnd_Click(object sender, RoutedEventArgs e)
        {
            _isSarted = false;

            _gameState = new GameState(PlayerColor.White, BoardModel.InitialBoard(_testEndFEN));

            DrawPieces(_gameState.Board);
        }

        private void Btn_PiecesMovemets_Click(object sender, RoutedEventArgs e)
        {
            var piecesMovemats = new PiecesMovementsWindow();

            piecesMovemats.Show();
        }
    }
} 
