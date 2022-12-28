using Cell = MineField.Model.Cells.Cell;

namespace MineFieldMaui.ViewModel
{
    public class MineCellViewModel : ViewModelBase
    {
        private int _verticalCoordinate;
        private int _horizontalCoordinate;
        private int _id;
        private bool _isRevealed;
        private bool _isFlagged;
        private bool _isBomb;
        private string _text;
        private Cell _cellToBind;
        public DelegateCommand RevealCellCommand { get; set; }
        public DelegateCommand FlagCellCommand { get; set; }

        public MineCellViewModel(Cell cellToBind, int id, DelegateCommand revealCommand, DelegateCommand flagCellCommand) : base()
        {
            _text = String.Empty;
            _cellToBind = cellToBind;
            _id = id;
            IsBomb = false;

            VerticalCoordinate = _cellToBind.VerticalPosition;
            HorizontalCoordinate = _cellToBind.HorizontalPosition;
            IsRevealed = _cellToBind.IsRevealed;
            if (IsRevealed)
            {
                string newNeighbourBombCount = _cellToBind.NeighbourBombCount == 0 ?
                    String.Empty :
                    _cellToBind.NeighbourBombCount.ToString();
                NeighbourBombCount = newNeighbourBombCount;
            }
            IsFlagged = _cellToBind.IsFlagged;

            RevealCellCommand = revealCommand;
            FlagCellCommand = flagCellCommand;
            _cellToBind.CellDataChanged += new EventHandler(OnCellDataChanged);
            FlagCellCommand = flagCellCommand;
        }

        private void OnCellDataChanged(object? sender, EventArgs e)
        {
            IsRevealed = _cellToBind.IsRevealed;
            if (IsRevealed)
            {
                IsBomb = _cellToBind.IsBomb();
                NeighbourBombCount = _cellToBind.NeighbourBombCount == 0 ? String.Empty : _cellToBind.NeighbourBombCount.ToString();
            }
            else
            {
                IsFlagged = _cellToBind.IsFlagged;
            }
        }

        public int ID
        {
            get { return _id; }
        }

        public int VerticalCoordinate
        {
            get { return _verticalCoordinate; }
            private set
            {
                _verticalCoordinate = value;
            }
        }
        public int HorizontalCoordinate
        {
            get { return _horizontalCoordinate; }
            private set
            {
                _horizontalCoordinate = value;
            }
        }

        public bool IsRevealed
        {
            get { return _isRevealed; }
            set
            {
                _isRevealed = value;
                if (value)
                {
                    IsBomb = _cellToBind.IsBomb();
                }
                OnPropertyChanged();
            }
        }
        public bool IsFlagged
        {
            get { return _isFlagged; }
            set
            {
                _isFlagged = value;
                OnPropertyChanged();
            }
        }

        public bool IsBomb
        {
            get { return _isBomb; }
            set
            {
                _isBomb = value;
                OnPropertyChanged();
            }
        }

        public string NeighbourBombCount
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}
