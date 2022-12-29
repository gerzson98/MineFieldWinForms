using MineField.Model;
using MineField.Model.MineFieldEventArgs;
using MineField.Model.Store;
using MineField.Persistence;
using MineFieldMaui.View;
using MineFieldMaui.ViewModel;

namespace MineFieldMaui
{
    public partial class AppShell : Shell
    {

        #region Properties

        private MineFieldPersistence _mineFieldPersistence;
        private MineFieldState _mineFieldState;
        private MineFieldViewModel _mineFieldViewModel;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;

        private String _gameName = "MineField game";
        private String _ok = "OK";

        #endregion

        #region Construction

        public AppShell
            (
            IStore mineFieldStore,
            MineFieldPersistence mineFieldPersistence,
            MineFieldState mineFieldState,
            MineFieldViewModel mineFieldViewModel
            )
        {
            InitializeComponent();
            _store = mineFieldStore;
            _mineFieldPersistence = mineFieldPersistence;
            _mineFieldState = mineFieldState;
            _mineFieldViewModel = mineFieldViewModel;

            BindEventHandlers();
            SetupStoring();
        }
        private void BindEventHandlers()
        {
            _mineFieldState.GameOver += MineFieldState_GameOver;
            _mineFieldViewModel.NewGame += MineFieldViewModel_NewGame;
            _mineFieldViewModel.RestartGame += MineFieldViewModel_RestartGame;
            _mineFieldViewModel.LoadGame += MineFieldViewModel_LoadGame;
            _mineFieldViewModel.SaveGame += MineFieldViewModel_SaveGame;
            _mineFieldViewModel.Settings += MineFieldViewModel_Settings;
        }

        private void SetupStoring()
        {
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        #endregion

        #region EventHandlers
        private async void MineFieldState_GameOver(object? sender, GameOverEventArgs eventArgs)
        {
            await DisplayAlert(_gameName + " is over", CalculateEndingMessage(eventArgs.EndingState), _ok);
            RestartGame();
        }
        private string CalculateEndingMessage(EndingState endingState)
        {
            if (endingState == EndingState.ItWasATie)
            {
                return "Congrats! You completed the game!";
            }
            else if (endingState == EndingState.FirstPlayerWon)
            {
                return $"{_mineFieldState.FirstPlayer.Name} won the game!";
            }
            return $"{_mineFieldState.SecondPlayer.Name} won the game!";
        }
        private void MineFieldViewModel_RestartGame(object? sender, EventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            int newGameSize = _mineFieldState.BoardSize;
            _mineFieldState = new MineFieldState(newGameSize, newGameSize);
            _mineFieldViewModel = new MineFieldViewModel(_mineFieldState);
            BindEventHandlers();
            SetupStoring();
            BindingContext = _mineFieldViewModel;
        }

        private async void MineFieldViewModel_NewGame(object? sender, NewGameEventArgs eventArgs)
        {
            await Navigation.PopAsync();
            _mineFieldState = new MineFieldState(eventArgs.GameSize, eventArgs.GameSize);
            _mineFieldViewModel = new MineFieldViewModel(_mineFieldState);
            BindEventHandlers();
            SetupStoring();
            BindingContext = _mineFieldViewModel;
        }
        private async void MineFieldViewModel_LoadGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }
        private async void MineFieldViewModel_SaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void MineFieldViewModel_Settings(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _mineFieldViewModel
            });
        }
        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();
            try
            {
                _mineFieldState = await _mineFieldPersistence.LoadGame(e.Name);
                _mineFieldViewModel = new MineFieldViewModel(_mineFieldState);
                BindEventHandlers();

                await Navigation.PopAsync();
                BindingContext = _mineFieldViewModel;
                await DisplayAlert(_gameName, "Successful load.", _ok);
            }
            catch
            {
                await DisplayAlert(_gameName, "Unsuccessful load.", _ok);
            }
        }
        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk
            try
            {
                await _mineFieldPersistence.SaveGame(e.Name, _mineFieldState);
                await DisplayAlert(_gameName, "Successful save.", _ok);
            }
            catch
            {
                await DisplayAlert(_gameName, "Unsuccessful save.", _ok);
            }
        }

        #endregion
    }
}