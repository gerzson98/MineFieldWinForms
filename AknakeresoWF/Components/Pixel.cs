using MineField.Model.Cells;

namespace AknakeresoWF.Design.Components
{
    public class Pixel: Button
    {
        public Cell CellInfo { get; private set; }

        public Pixel(Cell cellInfo)
        {
            CellInfo = cellInfo;
        }
    }
}
