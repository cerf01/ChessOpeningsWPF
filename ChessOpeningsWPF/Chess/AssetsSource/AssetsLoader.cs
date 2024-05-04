using ChessOpeningsWPF.Chess.Abstractions.Enums;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessOpeningsWPF.Chess.AssetsSource
{
    public static class AssetsLoader
    {
        private static ImageSource LoadSource(string path) =>
           new BitmapImage(new Uri(path, UriKind.Relative));

        private static ImageSource SetWhitePiece(PieceType type) =>
            LoadSource($"Chess/AssetsSource/Assets/WhitePieces/{type}W.png");

        private static ImageSource SetBlackPiece(PieceType type) =>
            LoadSource(($"Chess/AssetsSource/Assets/BlackPieces/{type}B.png"));

        public static ImageSource GetAsset(PlayerColor color, PieceType type) =>
            color == PlayerColor.White ? SetWhitePiece(type) : SetBlackPiece(type);
    }
}
