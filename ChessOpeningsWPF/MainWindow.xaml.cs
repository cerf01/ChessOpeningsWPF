using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Board;
using ChessOpeningsWPF.Chess.Movement;
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
using ChessOpeningsWPF.Chess;
using ChessOpeningsWPF.Chess.Abstractions.Enums;
using System.Linq;


namespace ChessOpeningsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public delegate void OnPositionSelect(Position position);

    public delegate void OnMove(SoundPlayer sound);



    public partial class MainWindow : Window
    {
        private List<List<Image>> _piecesAssets = new List<List<Image>>();

        private List<List<Rectangle>> _highlightsRectangles = new List<List<Rectangle>>();

        private Dictionary<Position, IMove> _movesCache = new Dictionary<Position, IMove>();

        private GameState _gameState;
       
        private SoundPlayer _soundPlayer = new SoundPlayer(System.IO.Path.GetFullPath("../../../Chess/AssetsSource/SoundAssets/MovePieceSound.wav"));

        private Position _selectedPosition;

        private bool _isUsed = false;

        private event OnPositionSelect _onFromPositionSelect;

        private event OnPositionSelect _onToPositionSelect;

        private event OnMove _onMove;

        private string _stardedFEN => "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq";


        public MainWindow MainApp { get => this; }

        public MainWindow()
        {

            InitializeComponent();

            InitialBoard();

            _gameState = new GameState(PlayerColor.White, BoardModel.InitialBoard(_stardedFEN));

            DrawPieces(_gameState.Board);

            _soundPlayer.Load();

            _onFromPositionSelect += OnFromPositionSelect;

            _onToPositionSelect += OnToPositionSelect;

            _onMove += OnMove;

        }

        private void OnMove(SoundPlayer sound)
        {
            sound.Play();
        }

        private void OnFromPositionSelect (Position position)
        {
            if (_gameState.Board[position] is null)
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
            _piecesAssets[r][c].Source = piece is null? new BitmapImage() : AssetsLoader.GetAsset(piece.Color, piece.Type);

        public void HandelMove(IMove move)
        {
            var moveToPositions = _gameState.MakeMove(move);

            if (moveToPositions is null)
                return;

            foreach (var position in moveToPositions)
                DrawPiece(_gameState.Board[position.Row, position.Column], position.Row, position.Column);

            RowValue.Text = $"{moveToPositions[0].Row}";
            ColumnValue.Text = $"{moveToPositions[0].Column}";

            ToRowValue.Text = $"{moveToPositions[1].Row}";
            ToColumnValue.Text = $"{moveToPositions[1].Column}";
          
            _onMove.Invoke(_soundPlayer);
        }

        public async Task HandelMoves(List<IMove> moves)
        {           
            if (_isUsed)
            {
                _gameState = new GameState(PlayerColor.White, BoardModel.InitialBoard(_stardedFEN));

                DrawPieces(_gameState.Board);
            }

            await Task.Delay(500);
            
            foreach (var move in moves)
            {
                HandelMove(move);

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

        private void SetMovesCache(List<IMove> moves)
        {
            _movesCache.Clear();

            foreach (var move in moves)
            {
                _movesCache[move.To] = move;
            }
        }

        private void ShowHighlights() 
        {
            Color color = Color.FromArgb(150, 125, 255, 125);
            foreach (var position in _movesCache.Keys)
            {
                _highlightsRectangles[position.Row][position.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (var position in _movesCache.Keys)            
                _highlightsRectangles[position.Row][position.Column].Fill = Brushes.Transparent;           
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
                             new NormalMove(new Position(6, 6), new Position(5,6))
                        };
                    }
                    break;
                case "opennding2":
                    {
                        moves = new List<IMove>()
                        {
                              new NormalMove(new Position(6, 3), new Position(4,3))
                        };                   
                    }
                    break;

                case "opennding3":
                    {
                        moves = new List<IMove>()
                        {
                              new NormalMove(new Position(6, 4), new Position(4,4)),
                               new NormalMove(new Position(1, 1), new Position(3,1)),
                        };
                    }
                    break;

                case "opennding4":
                    {
                        moves = new List<IMove>()
                        {
                            new NormalMove(new Position(6, 1), new Position(5,1)),
                            new NormalMove(new Position(1, 1), new Position(3,1)),
                              new NormalMove(new Position(6, 3), new Position(4,3))
                        };
                    }
                    break;
                default:
                    {
                        moves = new List<IMove>()
                        {
                            new NormalMove(new Position(0, 0), new Position(0,0))
                        };
                    }break;
            }
            
            await HandelMoves(moves);

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

            if(_selectedPosition is null)
                _onFromPositionSelect.Invoke(position);
            else
                _onToPositionSelect.Invoke(position);
            await Task.Delay(200);
            if (_gameState.CurrentTurn != _gameState.Player)
                HandelMove(_gameState.MakeComputerMove());
        }
    }
}