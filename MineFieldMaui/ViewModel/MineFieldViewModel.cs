using MineField.Model;
using System.Collections.ObjectModel;
using Cell = MineField.Model.Cells.Cell;

namespace MineFieldMaui.ViewModel
{
    public class MineFieldViewModel : ViewModelBase
    {
        private MineFieldState _mineFieldState;

        #region DelegateCommands
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand RestartGameCommand { get; set; }
        public DelegateCommand SettingsCommand { get; set; }
        public DelegateCommand LoadGameCommand { get; set; }
        public DelegateCommand SaveGameCommand { get; set; }
        #endregion

        #region EventHandlers
        public event EventHandler SaveGame = null!;
        public event EventHandler LoadGame = null!;
        public event EventHandler Settings = null!;
        public event EventHandler RestartGame = null!;
        public event EventHandler<NewGameEventArgs> NewGame = null!;
        #endregion

        #region NextPlayerLabel
        private string _nextPlayerLabel = String.Empty;
        public string NextPlayerLabel
        {
            get { return _nextPlayerLabel; }
            set
            {
                _nextPlayerLabel = value;
                OnPropertyChanged();
            }
        }
        private void OnStepChangeNextPlayerLabel(object? sender, EventArgs e)
        {
            NextPlayerLabel = CalculateNextPlayerLabel();
        }
        private string CalculateNextPlayerLabel()
        {
            string nextPlayerName = _mineFieldState.FirstPlayerIsNext ? _mineFieldState.FirstPlayer.Name : _mineFieldState.SecondPlayer.Name;
            return $"Next player is {nextPlayerName}.";
        }
        #endregion

        #region Difficulty
        private GameDifficultyViewModel _difficulty = null!;
        public ObservableCollection<GameDifficultyViewModel> DifficultyLevels { get; set; }
        public GameDifficultyViewModel Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                InitializeWithNewModel(new MineFieldState(Difficulty.Difficulty));
                OnPropertyChanged();
            }
        }

        #endregion

        #region GameDisplay
        public ObservableCollection<MineCellViewModel> Fields { get; set; }
        private int _gameSize;
        public int GameSize
        {
            get => _gameSize;
            set
            {
                _gameSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GameTableRows));
                OnPropertyChanged(nameof(GameTableColumns));
            }
        }
        // Segédpropertyk a tábla méretezéséhez

        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), GameSize).ToArray());
        }
        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), GameSize).ToArray());
        }
        #endregion

        #region EventForwardThrowers

        private void OnRestartGame()
        {
            RestartGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSettings()
        {
            Settings?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Construction
        public MineFieldViewModel(MineFieldState mineFieldState) : base()
        {
            InitializeWithNewModel(mineFieldState);
            NewGameCommand = new DelegateCommand(gameSizeString => { NewGame?.Invoke(this, new NewGameEventArgs(Convert.ToInt32(gameSizeString))); });
            LoadGameCommand = new DelegateCommand(_ => OnLoadGame());
            SaveGameCommand = new DelegateCommand(_ => OnSaveGame());
            SettingsCommand = new DelegateCommand(_ => OnSettings());
            RestartGameCommand = new DelegateCommand(_ => OnRestartGame());
            DifficultyLevels = new ObservableCollection<GameDifficultyViewModel>
            {
                new GameDifficultyViewModel { Difficulty = GameDifficulty.SMALL },
                new GameDifficultyViewModel { Difficulty = GameDifficulty.MEDIUM },
                new GameDifficultyViewModel { Difficulty = GameDifficulty.LARGE }
            };

        }

        private void InitializeWithNewModel(MineFieldState newModel)
        {
            _mineFieldState = newModel;
            Fields = new ObservableCollection<MineCellViewModel>();
            GameSize = newModel.BoardSize;
            NextPlayerLabel = CalculateNextPlayerLabel();
            BindFieldsFromModel();
        }

        private void BindFieldsFromModel()
        {
            int id = 0;
            for (int x = 0; x < _mineFieldState.BoardSize; x++)
            {
                for (int y = 0; y < _mineFieldState.BoardSize; y++)
                {
                    Cell nextCell = _mineFieldState[x, y];
                    nextCell.CellDataChanged += new EventHandler(OnStepChangeNextPlayerLabel);
                    MineCellViewModel newCell = new MineCellViewModel(
                        nextCell, id,
                        new DelegateCommand(cellId => RevealCell(Convert.ToInt32(cellId))),
                        new DelegateCommand(cellId => FlagCell(Convert.ToInt32(cellId)))
                        );
                    Fields.Add(newCell);
                    id++;
                }
            }
        }

        #endregion

        #region StepHelpers
        private void RevealCell(int cellId)
        {
            MineCellViewModel cellView = Fields[cellId];
            _mineFieldState[cellView.VerticalCoordinate, cellView.HorizontalCoordinate].Reveal(_mineFieldState.FirstPlayerIsNext);
        }
        private void FlagCell(int cellId)
        {
            MineCellViewModel cellView = Fields[cellId];
            _mineFieldState[cellView.VerticalCoordinate, cellView.HorizontalCoordinate].FlipFlagState();
        }
        #endregion

        private int getGameSizeFromDifficulty(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.SMALL:
                    return 6;
                case GameDifficulty.MEDIUM:
                    return 10;
                case GameDifficulty.LARGE:
                    return 16;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
