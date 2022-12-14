namespace MineField.Model.Cells
{
    public class Bomb : Cell
    {
        public Bomb(MineFieldState field, int verticalPosition, int horizontalPosition)
            : base(field, verticalPosition, horizontalPosition)
        {
            Type = CellType.BOMB;
        }

        public override bool IsBomb() { return true; }

        public Bomb(MineFieldState field, int verticalPosition, int horizontalPosition, bool isRevealed, bool isFlagged, int neighbourBombCount)
            : base(field, verticalPosition, horizontalPosition, isRevealed, isFlagged, neighbourBombCount)
        {
            Type = CellType.BOMB;
        }

        override public void Reveal(bool firstPlayerClicked)
        {
            IsRevealed = true;
            Field.InvokeGameOverOnDeath(firstPlayerClicked);
        }
    }
}
