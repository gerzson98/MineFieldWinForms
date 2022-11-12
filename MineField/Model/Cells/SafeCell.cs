using MineField.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineField.Model.Cells
{
    public class SafeCell : Cell
    {
        public SafeCell(MineFieldState field, int verticalPosition, int horizontalPosition)
            : base(field, verticalPosition, horizontalPosition) { Type = CellType.SAFE; }

        public SafeCell(MineFieldState field, int verticalPosition, int horizontalPosition, bool isRevealed, bool isFlagged, int neighbourBombCount)
            : base(field, verticalPosition, horizontalPosition, isRevealed, isFlagged, neighbourBombCount)
        {
            Type = CellType.SAFE;
        }

        public override bool IsBomb() { return false; }

        public override void Reveal(bool firstPlayerClicked)
        {
            if (!IsRevealed)
            {
                List<Cell> neighbourCells = GetNeighbourCells().ToList();
                NeighbourBombCount = neighbourCells.Where(cell => cell.IsBomb()).ToList().Count();
                IsRevealed = true;
                RegisterRevealedCell(firstPlayerClicked);
                if (NeighbourBombCount == 0)
                {
                    neighbourCells.ForEach(cell => { if (!cell.IsRevealed) { cell.Reveal(firstPlayerClicked); } });
                }
            }
        }
    }
}
