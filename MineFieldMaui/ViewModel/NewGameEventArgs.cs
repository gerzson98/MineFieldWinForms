using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineFieldMaui.ViewModel
{
    public class NewGameEventArgs : EventArgs
    {
        private int _gameSize;
        public int GameSize { get { return _gameSize; } }
        public NewGameEventArgs(int gameSize)
        {
            _gameSize = gameSize;
        }
    }
}
