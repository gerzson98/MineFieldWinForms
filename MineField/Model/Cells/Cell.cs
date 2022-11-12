using MineField.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineField.Model.Cells
{
    public abstract class Cell
    {
        protected MineFieldState Field { get; set; }
        public int VerticalPosition { get; private set; }
        public int HorizontalPosition { get; private set; }
        public bool IsFlagged { get; protected set; }
        public int NeighbourBombCount { get; set; }

        public bool IsRevealed { get; protected set; }
        public CellType Type { get; protected set; }

        public Cell(MineFieldState field, int verticalPosition, int horizontalPosition)
        {
            Field = field;
            VerticalPosition = verticalPosition;
            HorizontalPosition = horizontalPosition;
            IsRevealed = false;
            IsFlagged = false;
            NeighbourBombCount = 0;
        }

        public Cell(MineFieldState field, int verticalPosition, int horizontalPosition, bool isRevealed, bool isFlagged, int neighbourBombCount)
        {
            Field = field;
            VerticalPosition = verticalPosition;
            HorizontalPosition = horizontalPosition;
            IsRevealed = isRevealed;
            IsFlagged = isFlagged;
            NeighbourBombCount = neighbourBombCount;
        }

        abstract public void Reveal(bool firstPlayerClicked);
        abstract public bool IsBomb();

        public void FlipFlagState()
        {
            IsFlagged = !IsFlagged;
        }
        protected void RegisterRevealedCell(bool whoClicked)
        {
            Field.RegisterRevealedCell(whoClicked);
        }

        public List<Cell> GetNeighbourCells()
        {
            List<Cell> neighbourCells = new List<Cell>();
            foreach (int verticalDifference in Enumerable.Range(-1, 3))
            {
                foreach (int horizontalDifference in Enumerable.Range(-1, 3))
                {
                    if (!(horizontalDifference == 0 && verticalDifference == 0))
                    {
                        int targetVertical = VerticalPosition + verticalDifference;
                        int targetHorizontal = HorizontalPosition + horizontalDifference;
                        if (
                                Field.ContainsPosition(targetVertical, targetHorizontal) &&
                                !Field[targetVertical, targetHorizontal].IsRevealed
                            )
                        {
                            neighbourCells.Add(Field[targetVertical, targetHorizontal]);
                        }
                    }
                }
            }
            return neighbourCells;
        }
        public override string ToString()
        {
            int neighbourBombCount = GetNeighbourCells().Where(cell => cell.IsBomb()).ToList().Count();
            return $"{Type};{VerticalPosition},{HorizontalPosition};{IsRevealed};{IsFlagged};{neighbourBombCount}";
        }
    }
}
