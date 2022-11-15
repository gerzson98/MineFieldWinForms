using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MineField.Model;

namespace AknakeresoWpf
{
    /// <summary>
    /// Interaction logic for BoardDisplay.xaml
    /// </summary>
    public partial class BoardDisplay : Window
    {
        private MineFieldState _state { get; set; }
        public BoardDisplay(MineFieldState mineFieldGameState)
        {
            _state = mineFieldGameState;
            InitializeComponent();

            var boardRange = Enumerable.Range(0, _state.BoardSize);

            foreach (int verticalPosition in boardRange)
            {
                foreach (int horizontalPosition in boardRange)
                {
                    Pixel pixel = new Pixel(_state[horizontalPosition, verticalPosition]);

                    if (pixel._cellInfo.IsRevealed)
                    {
                        if (pixel._cellInfo.IsBomb())
                        {
                            pixel.Background = Brushes.Red;
                        }
                        else
                        {
                            pixel.Content = pixel._cellInfo.NeighbourBombCount;
                        }
                    }


                    pixel.Click += new RoutedEventHandler(Cell_MosueDown);
                    board.Children.Add(pixel);
                }
            }
        }

        private void Cell_MosueDown(object sender, RoutedEventArgs mouseEvent)
        {
            Pixel pixel = (Pixel)sender;
            pixel._cellInfo.Reveal(_state.FirstPlayerIsNext);
            if (pixel._cellInfo.IsRevealed)
            {
                if (pixel._cellInfo.IsBomb())
                {
                    pixel.Background = Brushes.Red;
                }
                else
                {
                    pixel.Content = pixel._cellInfo.NeighbourBombCount;
                }
            }
            mouseEvent.Handled = true;
        }
        private void OnPixelUpdated(Object sender, DataTransferEventArgs args)
        {
            Pixel pixel = (Pixel)sender;


        }
    }
}
