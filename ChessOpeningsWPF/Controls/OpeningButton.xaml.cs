using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Openings;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessOpeningsWPF.Controls
{
    /// <summary>
    /// Interaction logic for OpeningButton.xaml
    /// </summary>
    /// 
    public delegate Task OnClickMakeMoveDelegare(List<IMove> moves);
    public partial class OpeningButton : UserControl
    {
        private SolidColorBrush lightEnterBrush;
        private SolidColorBrush lightLeaveBrush;

        private bool _isHovered;

        public Border BgBrush {  get => ContentBorder; set => ContentBorder = value; }
        private readonly List<IMove> _moves = new List<IMove>();


        public event OnClickMakeMoveDelegare OnClick;
        public OpeningButton(ChessOpening opening)
        {
            InitializeComponent();

             Title.Text = opening.Name;
            _moves = opening.Moves;

            lightEnterBrush = new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0));

            _isHovered = false;

        }
        private void ChangeBackgroundColor() =>
            ContentBorder.Background = _isHovered ? lightEnterBrush : lightLeaveBrush;

        private void UserControl_MouseEnterOrLeave(object sender, MouseEventArgs e)
        {
            _isHovered = !_isHovered;
            ChangeBackgroundColor();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) =>
            OnClick.Invoke(_moves);
        
    }
}


