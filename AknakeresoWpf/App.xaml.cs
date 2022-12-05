using AknakeresoWPF.View;
using AknakeresoWPF.ViewModel;
using Microsoft.Win32;
using MineField.Model;
using MineField.Model.MineFieldEventArgs;
using MineField.Persistence;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AknakeresoWPF
{
    public partial class App : Application
    {
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private MineFieldState _model = null!;
        private MineFieldViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private MineFieldPersistence _persistence = null!;
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _view = new MainWindow();
            _persistence = new MineFieldPersistence();

            InitNewGame(InitNewModel(6));

            _view.Show();
        }

        private void OnGameOver(object? sender, GameOverEventArgs eventArgs)
        {
            EndingState endingState = eventArgs.EndingState;
            MessageBoxResult dialogResult = MessageBox.Show
                (
                    CalculateEndingMessage(endingState),
                    "Game ended",
                    MessageBoxButton.YesNo
                );
            if (dialogResult == MessageBoxResult.Yes)
            {
                InitNewGame(InitNewModel(6));
            }
            else { _view.Close(); }
        }

        private MineFieldState InitNewModel(int gameSize)
        {
            return new MineFieldState(gameSize, CalculateBombCount(gameSize));
        }

        private void InitNewGame(MineFieldState newModel)
        {
            _model = newModel;
            _viewModel = new MineFieldViewModel(_model);
            _viewModel.NewGame += new EventHandler<NewGameEventArgs>(OnNewGame);
            _viewModel.LoadGame += new EventHandler(OnLoadGame);
            _viewModel.SaveGame += new EventHandler(OnSaveGame);
            _view.DataContext = _viewModel;

            _model.GameOver += new EventHandler<GameOverEventArgs>(OnGameOver);
        }
        private void OnNewGame(object? s, NewGameEventArgs newGameEventArgs)
        {
            InitNewGame(InitNewModel(newGameEventArgs.GameSize));
        }

        private async void OnLoadGame(object? s, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "MineField LoadGame";
                if (openFileDialog.ShowDialog() == true)
                {
                    MineFieldState newModel = await _persistence.LoadGame(openFileDialog.FileName);
                    InitNewGame(newModel);
                }
            } catch (MineFieldDataException)
            {
                MessageBox.Show("Could not load the selected file!", "MineField", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private async void OnSaveGame(object? s, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "MineField SaveGame";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _persistence.SaveGame(saveFileDialog.FileName, _model);
                    }
                    catch (MineFieldDataException)
                    {
                        MessageBox.Show("Could not save the game!" + Environment.NewLine + "Bad filePath or probably we can't write to the chosen directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Could not save the game!", "MineField", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CalculateBombCount(int gameSize)
        {
            return (int)Math.Floor(gameSize * 0.9);
        }

        private string CalculateEndingMessage(EndingState endingState)
        {
            if (endingState == EndingState.ItWasATie)
            {
                return $"Congrats! You completed the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?";
            }
            else if (endingState == EndingState.FirstPlayerWon)
            {
                return $"{_model.FirstPlayer.Name} won the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?";
            }
            return $"{_model.SecondPlayer.Name} won the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?";
        }
    }
}
