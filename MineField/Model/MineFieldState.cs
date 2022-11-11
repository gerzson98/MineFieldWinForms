using MineField.Model.Cells;
using MineField.Model.Entities;

namespace MineField.Model
{
    public class MineFieldState
    {
        public static List<int> LEGIT_BOARD_SIZES = new List<int>() { 6, 10, 16 };
        private Cell[,] _cells { get; set; }
        public bool GameIsOver { get; internal set; }
        public int BoardSize { get; private set; }
        public int BombCount { get; private set; }
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }
        public bool FirstPlayerIsNext { get; private set; }
        private int RevealedCells { get; set; }
        public MineFieldState(int boardSize, int bombCount)
        {
            BoardSize = boardSize;
            BombCount = bombCount;
            GameIsOver = false;
            FirstPlayer = new Player("First player");
            SecondPlayer = new Player("Second player");
            FirstPlayerIsNext = true;
            _cells = new Cell[boardSize, boardSize];

            FillWithCells();
            SpreadBombs();
        }
        public MineFieldState(int boardSize, int bombCount, bool firstPlayerIsNext)
        {
            BoardSize = boardSize;
            BombCount = bombCount;
            GameIsOver = false;
            FirstPlayer = new Player("First player");
            SecondPlayer = new Player("Second player");
            FirstPlayerIsNext = firstPlayerIsNext;
            _cells = new Cell[boardSize, boardSize];
        }

        public bool ContainsPosition(int verticalPosition, int horizontalPosition)
        {
            var boardRange = Enumerable.Range(0, BoardSize);
            return boardRange.Contains(verticalPosition) && boardRange.Contains(horizontalPosition);
        }

        public Cell this[int verticalPosition, int horizontalPosition]
        {
            get { return _cells[verticalPosition, horizontalPosition]; }
            set { _cells[verticalPosition, horizontalPosition] = value; }
        }

        public bool GameEndedSuccessfully()
        {
            return (RevealedCells == (BoardSize * BoardSize - BombCount));
        }

        public void RegisterRevealedCell(bool whoClikced)
        {
            FirstPlayerIsNext = !whoClikced;
            IncrementRevealedCell();
            GameIsOver = GameEndedSuccessfully();
        }

        public List<Cell> GetNeighbourCells(Cell centerCell)
        {
            List<Cell> neighbourCells = new List<Cell>();
            foreach (int verticalDifference in Enumerable.Range(-1, 3))
            {
                foreach (int horizontalDifference in Enumerable.Range(-1, 3))
                {
                    if (!(horizontalDifference == 0 && verticalDifference == 0))
                    {
                        int targetVertical = centerCell.VerticalPosition + verticalDifference;
                        int targetHorizontal = centerCell.HorizontalPosition + horizontalDifference;
                        if (
                                ContainsPosition(targetVertical, targetHorizontal) &&
                                !this[targetVertical, targetHorizontal].IsRevealed
                            )
                        {
                            neighbourCells.Add(this[targetVertical, targetHorizontal]);
                        }
                    }
                }
            }
            return neighbourCells;
        }

        public void UpdateNeighbourCountsAndevealedCells()
        {
            foreach (Cell cell in _cells)
            {
                if (cell.IsRevealed)
                {
                    cell.NeighbourBombCount = GetNeighbourCells(cell).ToList()
                        .Where(cell => cell.IsBomb())
                        .ToList()
                        .Count();
                    IncrementRevealedCell();
                }
            }
        }

        private void IncrementRevealedCell()
        {
            RevealedCells++;
        }

        private void FillWithCells()
        {
            var range = Enumerable.Range(0, BoardSize);
            foreach (var horizontalCoordinate in range)
            {
                foreach (var verticalCoordinate in range)
                {
                    this[verticalCoordinate, horizontalCoordinate] = new SafeCell(this, verticalCoordinate, horizontalCoordinate);
                }
            }
        }
        private void SpreadBombs()
        {
            Random random = new Random();
            foreach (int _ in Enumerable.Range(1, BombCount))
            {
                bool foundANicePlaceForNextBomb = false;
                while (!foundANicePlaceForNextBomb)
                {
                    int potentialVertical = random.Next(BoardSize);
                    int potentialHorizontal = random.Next(BoardSize);
                    if (ContainsPosition(potentialVertical, potentialHorizontal) && !this[potentialVertical, potentialHorizontal].IsBomb())
                    {
                        this[potentialVertical, potentialHorizontal] = new Bomb(this, potentialVertical, potentialHorizontal);
                        foundANicePlaceForNextBomb = true;
                    }
                }
            }
        }

    }
}
