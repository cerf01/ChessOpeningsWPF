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
           new BitmapImage(new Uri(path));

        private static ImageSource SetWhitePiece(PieceType type) =>
            LoadSource(Path.GetFullPath($"../../../Chess/AssetsSource/Assets/WhitePieces/{type}W.png"));

        private static ImageSource SetBlackPiece(PieceType type) =>
            LoadSource(Path.GetFullPath($"../../../Chess/AssetsSource/Assets/BlackPieces/{type}B.png"));

        public static ImageSource GetAsset(PieceColor color, PieceType type) =>
            color == PieceColor.White ? SetWhitePiece(type) : SetBlackPiece(type);
    }
}
