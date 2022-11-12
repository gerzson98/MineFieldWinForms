using AknakeresoWF.Design.Components;
using MineField.Model;
using MineField.Persistence;

namespace AknakeresoWF
{
    public partial class BoardForm : Form
    {
        private MineFieldState Field { get; set; }
        private MineFieldPersistence PersistenceApi { get; set; }
        public BoardForm(MineFieldState field)
        {
            Field = field;
            PersistenceApi = new MineFieldPersistence();
            InitializeComponent();
            SetNextPlayerLabel();
            DrawMineField();
            RerenderDisplay();
        }

        private void SetNextPlayerLabel()
        {
            string nextPlayerName = Field.FirstPlayerIsNext ?
                Field.FirstPlayer.Name :
                Field.SecondPlayer.Name;
            label1.Text = $"Next player: {nextPlayerName}";
        }

        private void DrawMineField()
        {
            var range = Enumerable.Range(0, Field.BoardSize);
            int numberOfSize = 480 / Field.BoardSize;
            Size sizeOfPixel = new Size(numberOfSize, numberOfSize);
            foreach (int horizontalIndex in range)
            {
                foreach (int verticalIndex in range)
                {
                    Pixel currentPixel = new Pixel(Field[verticalIndex, horizontalIndex]);
                    currentPixel.Size = sizeOfPixel;
                    currentPixel.Location = new Point(horizontalIndex * numberOfSize, verticalIndex * numberOfSize);
                    currentPixel.MouseDown += CurrentPixel_MouseDown;
                    BoardDisplay.Controls.Add(currentPixel);
                };
            };
        }

        private void CurrentPixel_MouseDown(object? sender, EventArgs e)
        {
            if (sender is Pixel && e is MouseEventArgs)
            {
                Pixel clickedPixel = (Pixel)sender;
                MouseEventArgs mouseEvent = (MouseEventArgs)e;

                if (!clickedPixel.CellInfo.IsRevealed)
                {
                    if (mouseEvent.Button == MouseButtons.Left)
                    {
                        clickedPixel.CellInfo.Reveal(Field.FirstPlayerIsNext);
                        RerenderDisplay();
                        if (Field.GameIsOver)
                        {
                            if (Field.GameEndedSuccessfully())
                            {

                                MessageBox.Show(
                                    $"Congratulations!{Environment.NewLine}You tracked down all the safe spots!",
                                    "Well done!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                                Close();
                            }
                            else
                            {
                                string lostPlayerName = Field.FirstPlayerIsNext ?
                                Field.FirstPlayer.Name :
                                Field.SecondPlayer.Name;

                                clickedPixel.BackColor = Color.Red;
                                MessageBox.Show(
                                    $"Oopsies...{Environment.NewLine}{lostPlayerName} touched a bomb! You lost!",
                                    "BOOOOM!!!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Stop
                                );
                                Close();
                            }
                        }
                    }
                    if (mouseEvent.Button == MouseButtons.Right)
                    {
                        clickedPixel.CellInfo.FlipFlagState();
                        if (!clickedPixel.CellInfo.IsFlagged)
                        {
                            clickedPixel.Text = "";
                            clickedPixel.ForeColor = Color.Black;
                        }
                        else
                        {
                            PlaceFlagOnPixel(clickedPixel);
                        }
                    }
                }
            }
        }

        private void RerenderDisplay()
        {
            string nextPlayerName = Field.FirstPlayerIsNext ?
                                Field.FirstPlayer.Name :
                                Field.SecondPlayer.Name;
            label1.Text = $"Next player: {nextPlayerName}";
            foreach (Pixel currentPixel in BoardDisplay.Controls)
            {
                if (currentPixel.CellInfo.IsRevealed)
                {
                    currentPixel.Enabled = false;
                    if (currentPixel.CellInfo.NeighbourBombCount != 0)
                    {
                        currentPixel.Text = currentPixel.CellInfo.NeighbourBombCount.ToString();
                        currentPixel.ForeColor = Color.Black;
                    }
                }
                if (currentPixel.CellInfo.IsFlagged)
                {
                    PlaceFlagOnPixel(currentPixel);
                }
            }
        }

        private void PlaceFlagOnPixel(Pixel pixel)
        {
            pixel.Text = "B";
            pixel.ForeColor = Color.Red;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RerenderDisplay();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Browse Text Files",

                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                await PersistenceApi.SaveGame(fileName, Field);
            }
        }
    }
}