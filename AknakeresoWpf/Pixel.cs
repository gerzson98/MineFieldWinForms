using MineField.Model.Cells;
using System.Windows.Controls;

namespace AknakeresoWpf
{
    public class Pixel : Button {
        public Cell _cellInfo { get; private set; }
        public Pixel(Cell cellInfo)
        {
            _cellInfo = cellInfo;
        }
    }
}
