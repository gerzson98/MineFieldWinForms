using MineField.Model;
using MineField.Model.MineFieldEventArgs;
using MineField.Model.Store;
using MineField.Persistence;
using MineFieldMaui.ViewModel;

namespace MineFieldMaui
{
    public partial class AppShell : Shell
    {

        #region Fields

        private MineFieldPersistence _mineFieldPersistence;
        private MineFieldState _mineFieldState;
        private MineFieldViewModel _mineFieldViewModel;

        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        private readonly String _gameName = "MineField game";
        private readonly String _ok = "OK";

        #endregion

        #region Application methods

        public AppShell
            (
            IStore sudokuStore,
            MineFieldPersistence sudokuDataAccess,
            MineFieldState sudokuGameModel,
            MineFieldViewModel sudokuViewModel
            )
        {
            InitializeComponent();

            // játék összeállítása
            _store = sudokuStore;
            _mineFieldPersistence = sudokuDataAccess;
            _mineFieldState = sudokuGameModel;
            _mineFieldViewModel = sudokuViewModel;


            _mineFieldState.GameOver += MineFieldState_GameOver;

            _mineFieldViewModel.NewGame += MineFieldViewModel_NewGame;
            _mineFieldViewModel.LoadGame += MineFieldViewModel_LoadGame;
            _mineFieldViewModel.SaveGame += MineFieldViewModel_SaveGame;
            // TODO: Check this and refactor
            // _mineFieldViewModel.ExitGame += MineFieldViewModel_ExitGame;

            // a játékmentések kezelésének összeállítása
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        #endregion

        #region Model event handlers

        /// <summary>
        ///     Játék végének eseménykezelője.
        /// </summary>
        private async void MineFieldState_GameOver(object? sender, GameOverEventArgs eventArgs)
        {
                await DisplayAlert(_gameName, CalculateEndingMessage(eventArgs.EndingState), _ok);
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        ///     Új játék indításának eseménykezelője.
        /// </summary>
        private void MineFieldViewModel_NewGame(object? sender, EventArgs e)
        {
            _mineFieldState = .NewGame();
        }

        /// <summary>
        ///     Játék betöltésének eseménykezelője.
        /// </summary>
        private async void MineFieldViewModel_LoadGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }

        /// <summary>
        ///     Játék mentésének eseménykezelője.
        /// </summary>
        private async void MineFieldViewModel_SaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }

        private async void MineFieldViewModel_ExitGame(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _mineFieldViewModel
            }); // átnavigálunk a beállítások lapra
        }


        /// <summary>
        ///     Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                _mineFieldState = await _mineFieldPersistence.LoadGame(e.Name);

                // sikeres betöltés
                await Navigation.PopAsync(); // visszanavigálunk a játék táblára
                await DisplayAlert(_gameName, "Successful load.", _ok);
            }
            catch
            {
                await DisplayAlert(_gameName, "Successful load.", _ok);
            }
        }

        /// <summary>
        ///     Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk

            try
            {
                // elmentjük a játékot
                await _mineFieldPersistence.SaveGame(e.Name, _mineFieldState);
                await DisplayAlert(_gameName, "Successful save.", _ok);
            }
            catch
            {
                await DisplayAlert(_gameName, "Unsuccessful save.", _ok);
            }
        }


        private string CalculateEndingMessage(EndingState endingState)
        {
            if (endingState == EndingState.ItWasATie)
            {
                return $"Congrats! You completed the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?";
            }
            else if (endingState == EndingState.FirstPlayerWon)
            {
                return $"{_mineFieldState.FirstPlayer.Name} won the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?";
            }
            return $"{_mineFieldState.SecondPlayer.Name} won the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?";
        }

        #endregion
    }
}