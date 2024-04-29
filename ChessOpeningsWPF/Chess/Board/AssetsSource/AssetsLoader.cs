using ChessOpeningsWPF.Chess.Abstractions.Enums;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessOpeningsWPF.Chess.Board.AssetsSource
{
    public static class AssetsLoader
    {
        private static ImageSource LoadSource(string path) =>
           new BitmapImage(new Uri(path));

        private static ImageSource SetWhitePiece(PieceType type) 
        {
            switch (type) 
            {
                case PieceType.Pawn:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/WhitePieces/PawnW.png"));
                case PieceType.Rook:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/WhitePieces/RookW.png"));
                case PieceType.Bishop:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/WhitePieces/BishopW.png"));
                case PieceType.Knight:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/WhitePieces/KnightW.png"));
                case PieceType.Queen:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/WhitePieces/QueenW.png"));
                 case PieceType.King:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/WhitePieces/KingW.png"));
                default:
                    return null;
            }
        }

        private static ImageSource SetBlackPiece(PieceType type)
        {
            switch (type)
            {
                case PieceType.Pawn:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/BlackPieces/PawnB.png"));
                case PieceType.Rook:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/BlackPieces/RookB.png"));
                case PieceType.Bishop:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/BlackPieces/BishopB.png"));
                case PieceType.Knight:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/BlackPieces/KnightB.png"));
                case PieceType.Queen:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/BlackPieces/QueenB.png"));
                case PieceType.King:
                    return LoadSource(System.IO.Path.GetFullPath("../../../Chess/Board/AssetsSource/Assets/BlackPieces/KingB.png"));
                default:
                   return null;
            }
        }

        public static ImageSource GetAsset(PieceColor color, PieceType type)  =>
            color == PieceColor.White ? SetWhitePiece(type) : SetBlackPiece(type);


         
        
    }
}
