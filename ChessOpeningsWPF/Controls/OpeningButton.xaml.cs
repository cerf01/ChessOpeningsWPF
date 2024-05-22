using ChessOpeningsWPF.Chess.Abstractions.Interfaces;
using ChessOpeningsWPF.Chess.Openings;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
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

        private bool _isHovered;

        private readonly List<IMove> _moves = new List<IMove>();


        public event OnClickMakeMoveDelegare OnClick;
        public OpeningButton(ChessOpening opening)
        {
            InitializeComponent();

             Title.Text = opening.Name;
            _moves = opening.Moves;     
            
            _isHovered = false;

        }
        private void ChangeBackgroundColor()
        {
            if (_isHovered)
            {
                Title.Margin = new Thickness(10,10,10,10);
            }
            else 
            {
                Title.Margin = new Thickness(0);
            }
        }

        private void UserControl_MouseEnterOrLeave(object sender, MouseEventArgs e)
        {
            _isHovered = !_isHovered;
            ChangeBackgroundColor();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) =>
            OnClick.Invoke(_moves);
        
    }
}


