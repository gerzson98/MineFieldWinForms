namespace MineField.Model.MineFieldEventArgs
{
    public class GameOverEventArgs : EventArgs
    {
        public EndingState EndingState { get; private set; }

        public GameOverEventArgs(EndingState endingState)
        {
            EndingState = endingState;
        }
    }
}
