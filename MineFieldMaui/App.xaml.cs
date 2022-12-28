using MineField.Model;
using MineField.Persistence;
using MineFieldMaui.Persistence;
using MineFieldMaui.ViewModel;

namespace MineFieldMaui
{
    public partial class App : Application
    {
        /// <summary>
        /// Erre az útvonalra mentjük a félbehagyott játékokat
        /// </summary>
        private const string SuspendedGameSavePath = "SuspendedGame";

        private MineFieldState _mineFieldState;
        private readonly AppShell _appShell;
        private readonly MineFieldPersistence _mineFieldPersistence;
        private readonly IStore _mineFieldStore;
        private readonly MineFieldViewModel _mineFieldViewModel;

        public App()
        {
            InitializeComponent();

            _mineFieldStore = new MineFieldStore();
            _mineFieldPersistence = new MineFieldPersistence(FileSystem.AppDataDirectory);

            _mineFieldState = new MineFieldState(6, 6);
            _mineFieldViewModel = new MineFieldViewModel(_mineFieldState);

            _appShell = new AppShell(_mineFieldStore, _mineFieldPersistence, _mineFieldState, _mineFieldViewModel)
            {
                BindingContext = _mineFieldViewModel
            };
            MainPage = _appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            window.Created += (s, e) =>
            {
                _mineFieldState = new MineFieldState(6, 6);
            };

            window.Activated += (s, e) =>
            {
                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                    return;

                Task.Run(async () =>
                {
                    try
                    {
                        _mineFieldState = await _mineFieldPersistence.LoadGame(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            window.Stopped += (s, e) =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // elmentjük a jelenleg folyó játékot
                        await _mineFieldPersistence.SaveGame(SuspendedGameSavePath, _mineFieldState);
                    }
                    catch
                    {
                    }
                });
            };

            return window;
        }
    }
}