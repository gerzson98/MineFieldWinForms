using AknakeresoWPF.View;
using AknakeresoWPF.ViewModel;
using MineField.Model;
using MineField.Model.MineFieldEventArgs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AknakeresoWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private MineFieldState _model = null!;
        private MineFieldViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _view = new MainWindow();

            InitNewGame(6);

            _view.Show();
        }

        private void OnGameOver(object? sender, GameOverEventArgs eventArgs)
        {
            MessageBoxResult dialogResult;
            switch (eventArgs.EndingState)
            {
                case EndingState.FirstPlayerWon:
                    dialogResult = MessageBox.Show($"{_model.FirstPlayer.Name} won the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?", "Game ended", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        InitNewGame(10);
                    }
                    else { _view.Close(); }
                    return;
                case EndingState.SecondPlayerWon:
                    dialogResult = MessageBox.Show($"{_model.SecondPlayer.Name} won the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?", "Game ended", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        InitNewGame(10);
                    }
                    else { _view.Close(); }
                    return;
                case EndingState.ItWasATie:
                    dialogResult = MessageBox.Show($"Congrats! You completed the game!{Environment.NewLine + Environment.NewLine}Would you like to start a new game?", "Game ended", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        InitNewGame(10);
                    }
                    else { _view.Close(); }
                    return;
            }
        }

        private void InitNewGame(int gameSize)
        {
            _model = new MineFieldState(gameSize, CalculateBombCount(gameSize));
            _viewModel = new MineFieldViewModel(_model);
            _viewModel.NewGame += new EventHandler<NewGameEventArgs>(OnNewGame);
            _view.DataContext = _viewModel;

            _model.GameOver += new EventHandler<GameOverEventArgs>(OnGameOver);
        }
        private void OnNewGame(object? s, NewGameEventArgs newGameEventArgs)
        {
            InitNewGame(newGameEventArgs.GameSize);
        }

        private int CalculateBombCount(int gameSize)
        {
            return (int)Math.Floor(gameSize * 0.9);
        }
    }
}
