using MineField.Model.Cells;
using MineField.Model.MineFieldEventArgs;

namespace MineField.Model
{
    public class MineFieldState
    {
        private List<int> LEGIT_BOARD_SIZES = new List<int>() { 6, 10, 16 };
        private Cell[,] _cells { get; set; }
        public int BoardSize { get; private set; }
        internal int BombCount { get; private set; }
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }
        public bool FirstPlayerIsNext { get; private set; }
        public int RevealedCells { get; private set; }
        public event EventHandler<GameOverEventArgs> GameOver = null!;
        public MineFieldState(int boardSize, int bombCount)
        {
            if (!LEGIT_BOARD_SIZES.Contains(boardSize))
            {
                throw new ArgumentException();
            }
            BoardSize = boardSize;
            BombCount = bombCount;
            FirstPlayer = new Player("First player");
            SecondPlayer = new Player("Second player");
            FirstPlayerIsNext = true;
            _cells = new Cell[boardSize, boardSize];

            FillWithCells();
            SpreadBombs();
        }
        public MineFieldState(int boardSize, int bombCount, bool firstPlayerIsNext, int revealedCells)
        {
            if (!LEGIT_BOARD_SIZES.Contains(boardSize))
            {
                throw new ArgumentException();
            }
            BoardSize = boardSize;
            BombCount = bombCount;
            FirstPlayer = new Player("First player");
            SecondPlayer = new Player("Second player");
            FirstPlayerIsNext = firstPlayerIsNext;
            _cells = new Cell[boardSize, boardSize];
            RevealedCells = revealedCells;
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
        public void InvokeGameOverOnDeath(bool firstPlayerLost)
        {
            EndingState playerFlag = firstPlayerLost ? EndingState.SecondPlayerWon : EndingState.FirstPlayerWon;
            GameOver?.Invoke(this, new GameOverEventArgs(playerFlag));
        }

        public void RegisterRevealedCell(bool whoClikced)
        {
            FirstPlayerIsNext = !whoClikced;
            IncrementRevealedCell();
            if (GameEndedSuccessfully())
            {
                GameOver?.Invoke(this, new GameOverEventArgs(EndingState.ItWasATie));
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
