using MineField.Model;
using System.Collections.ObjectModel;
using Cell = MineField.Model.Cells.Cell;

namespace MineFieldMaui.ViewModel
{
    public class MineFieldViewModel : ViewModelBase
    {
        private MineFieldState _mineFieldState;
        private int _gameSize;
        private string _nextPlayerLabel = String.Empty;
        public DelegateCommand NewGameCommand { get; set; }
        public event EventHandler<NewGameEventArgs> NewGame = null!;
        public DelegateCommand LoadGameCommand { get; set; }
        public event EventHandler LoadGame = null!;
        public DelegateCommand SaveGameCommand { get; set; }
        public event EventHandler SaveGame = null!;

        public string NextPlayerLabel
        {
            get { return _nextPlayerLabel; }
            set
            {
                _nextPlayerLabel = value;
                OnPropertyChanged();
            }
        }
        public int GameSize { get { return _gameSize; } private set { _gameSize = value; OnPropertyChanged(); } }

        public ObservableCollection<MineCellViewModel> Fields { get; set; }
        public MineFieldViewModel(MineFieldState mineFieldState) : base()
        {
            _mineFieldState = mineFieldState;
            NextPlayerLabel = CalculateNextPlayerLabel();
            GameSize = _mineFieldState.BoardSize;
            Fields = new ObservableCollection<MineCellViewModel>();
            NewGameCommand = new DelegateCommand(gameSizeString => { NewGame?.Invoke(this, new NewGameEventArgs(Convert.ToInt32(gameSizeString))); });
            LoadGameCommand = new DelegateCommand(_ => { LoadGame?.Invoke(this, EventArgs.Empty); });
            SaveGameCommand = new DelegateCommand(_ => { SaveGame?.Invoke(this, EventArgs.Empty); });
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

        private void OnStepChangeNextPlayerLabel(object? sender, EventArgs e)
        {
            NextPlayerLabel = CalculateNextPlayerLabel();
        }
        private string CalculateNextPlayerLabel()
        {
            string nextPlayerName = _mineFieldState.FirstPlayerIsNext ? _mineFieldState.FirstPlayer.Name : _mineFieldState.SecondPlayer.Name;
            return $"Next player is {nextPlayerName}.";
        }

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
    }
}
