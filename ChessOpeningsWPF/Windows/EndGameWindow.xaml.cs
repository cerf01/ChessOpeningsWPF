using ChessOpeningsWPF.Chess;
using ChessOpeningsWPF.Chess.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ChessOpeningsWPF
{
    /// <summary>
    /// Interaction logic for EndGameWindow.xaml
    /// </summary>
    public partial class EndGameWindow : Window
    { 

        private bool _isHoverd;

        public bool TryAgain {  get;private set; }

        public EndGameWindow(Window owner, GameResult result)
        {
            Owner = owner;

            _isHoverd = false;

            InitializeComponent();
            if (result is not null)
            {
                TxtBl_GameResult.Text = result.Winner == PlayerColor.None ? "Draw" : $"Winner is {result.Winner}";
                TxtBl_Reason.Text = result.Reason.ToString();
            }
        }
        private void ChangeBackgroundColor(Border button)
        {
            if (_isHoverd)
                button.Margin = new Thickness(35,40,35,40);
            else
                button.Margin = new Thickness(40);
        }


        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            TryAgain = true;
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            TryAgain = false;
            Close();
        }

        private void MouseLeaveOrEnter(object sender, MouseEventArgs e)
        {
            _isHoverd = !_isHoverd;
            ChangeBackgroundColor((Border)sender);
        }
    }
}
