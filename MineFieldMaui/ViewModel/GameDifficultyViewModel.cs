using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineFieldMaui.ViewModel
{
    public class GameDifficultyViewModel : ViewModelBase
    {
        private GameDifficulty _difficulty;

        public GameDifficulty Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DifficultyText));
            }
        }

        public string DifficultyText => _difficulty.ToString();
    }
}
