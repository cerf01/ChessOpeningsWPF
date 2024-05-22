using ChessOpeningsWPF.Chess.Abstractions.Enums;
using ChessOpeningsWPF.Chess.AssetsSource;
using ChessOpeningsWPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessOpeningsWPF.Windows
{
    /// <summary>
    /// Interaction logic for PiecesMovementsWindow.xaml
    /// </summary>
    public partial class PiecesMovementsWindow : Window
    {

        private static List<PieceMoveInfo> _pawnMovesInfo => new List<PieceMoveInfo>()
        {
            new PieceMoveInfo("Move", "Pawns can only move forward one square at a time, except for their very first move where they can move forward two squares", "/Chess/Source/Assets/Moves/Pawn/Move.png"),
            new PieceMoveInfo("Attack", "Pawns can only capture one square diagonally in front of them", "/Chess/Source/Assets/Moves/Pawn/Attack.png"),
            new PieceMoveInfo("En Passant", "If a pawn moves out two squares on its first move, and by doing so lands to the side of an opponent's pawn (effectively jumping past the other pawn's ability to capture it), that other pawn has the option of capturing the first pawn as it passes by", "/Chess/Source/Assets/Moves/Pawn/EnPassant.png"),
            new PieceMoveInfo("Promotion", " If a pawn reaches the other side of the board it can become other chess piece(Queen)", "/Chess/Source/Assets/Moves/Pawn/PawnPromotion.png"),
        };

        private static List<PieceMoveInfo> _kingMoves => new List<PieceMoveInfo>()
        {
            new PieceMoveInfo("Move and Attack", "The king can only move one square in any direction - up, down, to the sides, and diagonally", "/Chess/Source/Assets/Moves/King/Move.png"),
            new PieceMoveInfo("Castling", " This move allows you to do two important things all in one move: get your king to safety (hopefully), and get your rook out of the corner and into the game. On a player's turn he may move his king two squares over to one side and then move the rook from that side's corner to right next to the king on the opposite side",  "/Chess/Source/Assets/Moves/King/Castling.png")
        };

        private static List<PieceMoveInfo> _queenMoves => new List<PieceMoveInfo>()
        {
        new PieceMoveInfo("Move and Attack", "Queen can move in any one straight direction - forward, backward, sideways, or diagonally - as far as possible as long as she does not move through any of her own pieces", "/Chess/Source/Assets/Moves/Queen/Move.png")
        };

        private static List<PieceMoveInfo> _rookMoves => new List<PieceMoveInfo>()
        {
            new PieceMoveInfo("Move and attack", "The rook may move as far as it wants, but only forward, backward, and to the sides", "/Chess/Source/Assets/Moves/Rook/Move.png")
        };
        private static List<PieceMoveInfo> _bishopMioves => new List<PieceMoveInfo>()
             {
                 new PieceMoveInfo("Move and Attack", "The bishop may move as far as it wants, but only diagonally. Each bishop starts on one color (light or dark) and must always stay on that color", "/Chess/Source/Assets/Moves/Bishop/Move.png")
             };
        private static List<PieceMoveInfo> _knightsMoves => new List<PieceMoveInfo>()
             {
                 new PieceMoveInfo("Move and Attack", "Knights move in a very different way from the other pieces – going two squares in one direction, and then one more move at a 90-degree angle, just like the shape of an 'L'", "/Chess/Source/Assets/Moves/Knight/Move.png")
             };

        private List<PieceMoveInfo> _currentPieceMoves;
        public PiecesMovementsWindow()
        {
            InitializeComponent();
            _currentPieceMoves = new List<PieceMoveInfo>();
            Btn_Pawn.Img_Piece.Source = AssetsLoader.GetAsset(PlayerColor.White, PieceType.Pawn);
            Btn_Knight.Img_Piece.Source = AssetsLoader.GetAsset(PlayerColor.White, PieceType.Night);
            Btn_King.Img_Piece.Source = AssetsLoader.GetAsset(PlayerColor.White, PieceType.King);
            Btn_Queen.Img_Piece.Source = AssetsLoader.GetAsset(PlayerColor.White, PieceType.Queen);
            Btn_Rook.Img_Piece.Source = AssetsLoader.GetAsset(PlayerColor.White, PieceType.Rook);
            Btn_Bishop.Img_Piece.Source = AssetsLoader.GetAsset(PlayerColor.White, PieceType.Bishop);

         


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hi!");
        }

        private void Btn_Pawn_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UpdatePanel() 
        {
            Panel_Info.Children.Clear();
            foreach (var info in _currentPieceMoves)
            {
                Panel_Info.Children.Add(info);
            }
        }

        private void Btn_Pawn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPieceMoves.Clear();
            _currentPieceMoves = _pawnMovesInfo;
            UpdatePanel();
        }

        private void Btn_Knight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPieceMoves.Clear();
            _currentPieceMoves = _knightsMoves;
            UpdatePanel();
        }

        private void Btn_Bishop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPieceMoves.Clear();
            _currentPieceMoves = _bishopMioves;
            UpdatePanel();
        }

        private void Btn_Rook_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPieceMoves.Clear();
            _currentPieceMoves = _rookMoves;
            UpdatePanel();
        }

        private void Btn_Queen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPieceMoves.Clear();
            _currentPieceMoves = _queenMoves;
            UpdatePanel();
        }

        private void Btn_King_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentPieceMoves.Clear();
            _currentPieceMoves = _kingMoves;
            UpdatePanel();
        }
    }
}
