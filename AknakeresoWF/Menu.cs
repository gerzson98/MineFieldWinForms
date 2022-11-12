using MineField.Model;
using MineField.Persistence;

namespace AknakeresoWF.Design
{
    public partial class Menu : Form
    {
        private MineFieldPersistence PersistenceApi { get; set; }
        public Menu()
        {
            InitializeComponent();
            PersistenceApi = new MineFieldPersistence();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Title = "Browse Text Files",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "txt",
                    Filter = "txt files (*.txt)|*.txt",
                    FilterIndex = 2,
                    RestoreDirectory = true,

                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog1.FileName;
                    MineFieldState loadedGame = await PersistenceApi.LoadGame(fileName);
                    BoardForm boadForm = new BoardForm(loadedGame);
                    boadForm.Show();
                }
            }
            catch (MineFieldDataException _)
            {
                MessageBox.Show(
                                    "You chose a file, that is not valid, returning to the Menu.",
                                    "Not valid save!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            BoardForm boardForm = new BoardForm(new MineFieldState(6, 3));
            boardForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BoardForm boardForm = new BoardForm(new MineFieldState(10, 15));
            boardForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BoardForm boardForm = new BoardForm(new MineFieldState(16, 24));
            boardForm.Show();
        }
    }
}
