namespace MineField.Model.Cells
{
    public abstract class Cell
    {
        protected MineFieldState Field { get; set; }
        public int VerticalPosition { get; private set; }
        public int HorizontalPosition { get; private set; }
        private bool _isFlagged;
        public bool IsFlagged { get { return _isFlagged; } protected set { _isFlagged = value; RaiseCellDataChangedEvent(); } }
        public int NeighbourBombCount { get; protected set; }
        private bool _isRevealed;
        public bool IsRevealed { get { return _isRevealed; } protected set { _isRevealed = value; RaiseCellDataChangedEvent(); } }
        public CellType Type { get; protected set; }
        public event EventHandler? CellDataChanged;

        public Cell(MineFieldState field, int verticalPosition, int horizontalPosition)
        {
            Field = field;
            VerticalPosition = verticalPosition;
            HorizontalPosition = horizontalPosition;
            _isRevealed = false;
            _isFlagged = false;
            NeighbourBombCount = 0;
        }

        public Cell(MineFieldState field, int verticalPosition, int horizontalPosition, bool isRevealed, bool isFlagged, int neighbourBombCount)
        {
            Field = field;
            VerticalPosition = verticalPosition;
            HorizontalPosition = horizontalPosition;
            _isRevealed = isRevealed;
            _isFlagged = isFlagged;
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

        private void RaiseCellDataChangedEvent()
        {
            CellDataChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
