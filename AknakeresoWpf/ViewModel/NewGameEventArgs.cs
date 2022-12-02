using System;

namespace AknakeresoWPF.ViewModel
{
    public class NewGameEventArgs: EventArgs
    {
        private int _gameSize;
        public int GameSize { get { return _gameSize; } }
        public NewGameEventArgs(int gameSize)
        {
            _gameSize = gameSize;
        }
    }
}
