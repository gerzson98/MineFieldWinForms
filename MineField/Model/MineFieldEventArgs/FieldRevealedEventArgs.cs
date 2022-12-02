using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineField.Model.MineFieldEventArgs
{
    public class FieldRevealedEventArgs: EventArgs
    {
        public int VerticalCoordinate { get; private set; }
        public int HorizontalCoordinate { get; private set; }

        public FieldRevealedEventArgs(int verticalCoordinate, int horizontalCoordinate)
        {
            VerticalCoordinate = verticalCoordinate;
            HorizontalCoordinate = horizontalCoordinate;
        }
    }
}
