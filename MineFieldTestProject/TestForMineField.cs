using MineField.Model;
using MineField.Model.Cells;

namespace MineFieldTestProject
{
    [TestClass]
    public class TestForMineField
    {
        private IEnumerable<Int32> helperRange = Enumerable.Range(0, 6);

        [TestMethod]
        public void MineField_BasicConstructTest_Small()
        {
            int plannedBombCount = 8;
            int plannedBoardSize = 6;
            MineFieldState mineField = new MineFieldState(plannedBoardSize, plannedBombCount);

            int actualBombCount = 0;
            foreach (int x in helperRange)
            {
                foreach (int y in helperRange)
                {
                    if (mineField[x, y].IsBomb())
                    {
                        actualBombCount++;
                    }
                    Assert.AreEqual(mineField[x, y].IsRevealed, false);
                }
            }

            Assert.AreEqual(plannedBombCount, actualBombCount);
            Assert.AreEqual(plannedBoardSize, mineField.BoardSize);
        }

        [TestMethod]
        public void MineField_BasicConstructTest_Medium()
        {
            int plannedBombCount = 10;
            int plannedBoardSize = 10;
            MineFieldState mineField = new MineFieldState(plannedBoardSize, plannedBombCount);

            int actualBombCount = 0;
            foreach (int x in helperRange)
            {
                foreach (int y in helperRange)
                {
                    if (mineField[x, y].IsBomb())
                    {
                        actualBombCount++;
                        Assert.AreEqual(mineField[x, y].IsRevealed, false);
                    }
                }
            }

            Assert.AreEqual(actualBombCount, plannedBombCount);
            Assert.AreEqual(plannedBoardSize, mineField.BoardSize);
        }

        [TestMethod]
        public void MineField_BasicConstructTest_Big()
        {
            int plannedBombCount = 18;
            int plannedBoardSize = 16;
            MineFieldState mineField = new MineFieldState(plannedBoardSize, plannedBombCount);

            int actualBombCount = 0;
            foreach (int x in helperRange)
            {
                foreach (int y in helperRange)
                {
                    if (mineField[x, y].IsBomb())
                    {
                        actualBombCount += 1;
                    }
                    Assert.AreEqual(mineField[x, y].IsRevealed, false);
                }
            }

            Assert.AreEqual(plannedBombCount, actualBombCount);
            Assert.AreEqual(plannedBoardSize, mineField.BoardSize);
        }

        [TestMethod]
        public void MineFieldBombClickTest()
        {

            MineFieldState mineField = new MineFieldState(6, 12);
            foreach (int x in helperRange)
            {
                bool shouldBreak = false;
                foreach (int y in helperRange)
                {
                    if (mineField[x, y].IsBomb())
                    {
                        mineField[x, y].Reveal(true);
                        Assert.AreEqual(mineField.GameIsOver, true);
                        Assert.AreEqual(mineField.GameEndedSuccessfully(), false);
                        shouldBreak = true;
                        break;
                    }
                }
                if (shouldBreak) { break; }
            }
        }

        [TestMethod]
        public void MineField_SuccessTest()
        {

            MineFieldState mineField = new MineFieldState(6, 4);
            foreach (int x in helperRange)
            {
                foreach (int y in helperRange)
                {
                    if (!mineField[x, y].IsBomb())
                    {
                        mineField[x, y].Reveal(true);
                    }
                }
            }

            Assert.AreEqual(mineField.GameIsOver, true);
            Assert.AreEqual(mineField.GameEndedSuccessfully(), true);
        }
        [TestMethod]
        public void MineField_GetNeighbourCells_Center()
        {
            MineFieldState mineField = new MineFieldState(6, 4);
            Cell centerCell = mineField[2, 2];

            List<Cell> expectedNeighbourCells = new List<Cell>(){
                mineField[1, 1],
                mineField[1, 2],
                mineField[1, 3],
                mineField[2, 1],
                mineField[2, 3],
                mineField[3, 1],
                mineField[3, 2],
                mineField[3, 3],
            };

            List<Cell> resultNeighbourCells = mineField.GetNeighbourCells(centerCell);

            Assert.AreEqual(expectedNeighbourCells.Count, resultNeighbourCells.Count);

            foreach (Cell cell in expectedNeighbourCells)
            {
                Assert.IsTrue(resultNeighbourCells.Contains(cell));
            }
        }

        [TestMethod]
        public void MineField_GetNeighbourCells_Edge()
        {
            MineFieldState mineField = new MineFieldState(6, 4);
            Cell centerCell = mineField[0, 0];

            List<Cell> expectedNeighbourCells = new List<Cell>(){
                mineField[0, 1],
                mineField[1, 1],
                mineField[1, 0]
            };

            List<Cell> resultNeighbourCells = mineField.GetNeighbourCells(centerCell);

            Assert.AreEqual(expectedNeighbourCells.Count, resultNeighbourCells.Count);

            foreach (Cell cell in expectedNeighbourCells)
            {
                Assert.IsTrue(resultNeighbourCells.Contains(cell));
            }
        }

        [TestMethod]
        public void Reveal_DoesNotRevealOthersWhenHasBombNextToIt()
        {
            MineFieldState mineField = new MineFieldState(6, 10);

            Cell cellToReveal = null!;

            foreach (int x in helperRange)
            {
                bool shouldBreak = false;
                foreach (int y in helperRange)
                {
                    if (!mineField[x, y].IsBomb() && mineField.GetNeighbourCells(mineField[x, y]).Any(cell => cell.IsBomb()))
                    {
                        cellToReveal = mineField[x, y];
                        shouldBreak = true;
                        break;
                    }
                }
                if (shouldBreak) { break; }
            }

            cellToReveal.Reveal(true);
            foreach (int x in helperRange)
            {
                foreach (int y in helperRange)
                {
                    if (x != cellToReveal.VerticalPosition || y != cellToReveal.HorizontalPosition)
                    {
                        Assert.IsFalse(mineField[x, y].IsRevealed);
                    }
                }
            }
        }
    }
}