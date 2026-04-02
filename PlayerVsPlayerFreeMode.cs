using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace ModifiedConnect4
{
    // Enumeration representing players
    internal enum Player
    {
        None = 0,
        Player1 = 1,
        Player2 = 2
    }

    // Abstract base class for game boards
    internal abstract class PlayerVsPlayerFreeMode : Form
    {
        // Common constants and variables
        private Options options;
        protected const int Rows = 6;
        protected const int Columns = 7;
        protected int[,] board = new int[Rows, Columns];
        protected Player currentPlayer = Player.Player1;


        //public PlayerVsPlayerFreeMode(Options options)
        //{
            
        //    InitializeBoard();
        //}

        // UI elements
        protected TableLayoutPanel tableLayoutPanel;
        protected Button[,] buttons;
        protected Label label;
        protected Dictionary<Player, int> playerScores;

        // Abstract methods to be implemented by derived classes
        protected abstract void MainFrame();
        protected abstract void InitializeBoard();
        protected abstract void ScoreDisplay();
        protected abstract void DropChip(object sender, EventArgs e);
        protected abstract int GetAvailableRow(int col);
        protected abstract void SetButtonAppearance(Button btn);
        protected abstract void DrawCircle(object sender, PaintEventArgs e);
        protected abstract void SetTableLayoutPanelStyles();
        protected abstract bool CheckForWin(int row, int col);
        protected abstract bool CheckDirection(int row, int col, int rowIncrement, int colIncrement);
        protected abstract bool IsInBounds(int row, int col);
        protected abstract void IncrementPlayerScore();
        protected abstract void ResetBoard();
    }

    // Concrete class for Connect 4 board
    internal class PlayerVsPlayerFree : PlayerVsPlayerFreeMode
    {
        // Properties to store selected options
        public Color ChipColorPlayer1 { get; set; }
        public Color ChipColorPlayer2 { get; set; }
        public bool EnableSounds { get; set; }
        public string SelectedSoundFile { get; set; }

        private Options options = new Options();
        private SoundPlayer chipDropSound = new SoundPlayer("Pop_Sound_Effect.wav");

        private string playerName1;
        private string playerName2;
        private string[] playerName;

        // Constructor
        public PlayerVsPlayerFree(string[] playerName, Options options)
        {
            this.playerName1 = playerName[0];
            this.playerName2 = playerName[1];
            this.options = options;
            InitializeBoard();
            MainFrame();
            ReturnToMainMenuBtn();
            SaveScoreBtn();
            ScoreDisplay();
            playerScores = new Dictionary<Player, int>
            {
                { Player.Player1, 0 },
                { Player.Player2, 0 }
            };
        }


        // Implementation of abstract methods
        protected override void MainFrame()
        {
            // Set up main frame properties
            Text = "Connect 4 Game";
            ClientSize = new Size(720, 720);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.MidnightBlue;
        }

        protected override void InitializeBoard()
        {
            // Set up the game board UI
            tableLayoutPanel = new TableLayoutPanel
            {
                Size = new Size(0, 500),
                Dock = DockStyle.Bottom,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                RowCount = Rows,
                ColumnCount = Columns
            };

            buttons = new Button[Rows, Columns];

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    buttons[row, col] = new Button
                    {
                        Dock = DockStyle.Fill,
                        Tag = col
                    };
                    buttons[row, col].Click += DropChip;

                    SetButtonAppearance(buttons[row, col]);
                    tableLayoutPanel.Controls.Add(buttons[row, col], col, row);
                }
            }
            SetTableLayoutPanelStyles();

            Controls.Add(tableLayoutPanel);
        }
        //Function for CREATING RETURNING TO MAIN MENU BTN
        public void ReturnToMainMenuBtn()
        {
            Button returnMenu = new Button();

            returnMenu.Text = "Return to Menu";

            returnMenu.Click += new EventHandler(ReturnClickBtn);
            returnMenu.BackColor = Color.White;
            returnMenu.Font = new Font("Rockwell", 9, FontStyle.Regular);
            returnMenu.FlatStyle = FlatStyle.Popup;
            returnMenu.Size = new Size(120, 20);
            this.Controls.Add(returnMenu);

        }

        //EVENT (AFTER CLICKING RETURN MAIN MENU BTN)
        public void ReturnClickBtn(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Returning to Main Menu without finishing the game will not save your data", "Warning", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                MainMenu returnMenu = new MainMenu();
                returnMenu.Show();
                this.Hide();
            }

        }



        public void SaveScoreBtn()
        {
            Button saveScore = new Button();

            saveScore.Text = "Save Score";

            saveScore.Click += new EventHandler(SaveScoreClick);
            saveScore.BackColor = Color.White;
            saveScore.Font = new Font("Rockwell", 9, FontStyle.Regular);
            saveScore.FlatStyle = FlatStyle.Popup;
            saveScore.Size = new Size(120, 20);
            saveScore.Location = new Point(105, 0);
            this.Controls.Add(saveScore);

        }
        //SAVE Score
        public void SaveScoreClick(object sender, EventArgs e)
        {
                SaveScoreTextFile();
    
        }
        //SAVING the score to the text file
        private void SaveScoreTextFile()
        {
            int player1Win = playerScores[Player.Player1] / 2;
            int player2Win = playerScores[Player.Player2] / 2;

            try
            {
                // dir for the exe
                string filePath = "PlayerScore.txt";

                
                using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    
                    writer.WriteLine($"{playerName1},{playerScores[Player.Player1]},{player1Win}");

                    
                    writer.WriteLine($"{playerName2},{playerScores[Player.Player2]},{player2Win}");
                }

                MessageBox.Show("Scores saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving scores: {ex.Message}");
            }
        }

        protected override void ScoreDisplay()
        {
            // Set up the score display label
            label = new Label
            {
                Text = "SCORE",
                Font = new Font("Rockwell", 28, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(725, 72)
            };

            Controls.Add(label);
        }

        protected override void DropChip(object sender, EventArgs e)
        {
            // Handle dropping a chip when a button is clicked
            Button btn = (Button)sender;
            int col = (int)btn.Tag;
            chipDropSound.Play();

            if (!btn.Enabled)
            {
                MessageBox.Show("Column is full. Choose another column.");
                return;
            }

            int row = GetAvailableRow(col);

            if (row != -1)
            {
                board[row, col] = (int)currentPlayer;
                btn.BackColor = (currentPlayer == Player.Player1) ? Color.Red : Color.Yellow;

                btn.Enabled = false;

                if (CheckForWin(row, col))
                {
                    IncrementPlayerScore();
                }
                else
                {
                    currentPlayer = (currentPlayer == Player.Player1) ? Player.Player2 : Player.Player1;
                }
            }
            else
            {
                MessageBox.Show("Column is full. Choose another column.");
                
            }
        }

        protected override int GetAvailableRow(int col)
        {
            // Get the available row for placing a chip in the specified column
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (board[row, col] == 0)
                    return row;
            }
            return -1;
        }

        protected override void SetButtonAppearance(Button btn)
        {
            // Set the appearance of a button (e.g., flat style, size, paint event)
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.Size = new Size(50, 50);
            btn.Paint += DrawCircle;
        }

        protected override void DrawCircle(object sender, PaintEventArgs e)
        {
            // Draw a circle inside a button
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.FillEllipse(new SolidBrush(btn.BackColor), 0, 0, btn.Width, btn.Height);
        }

        protected override void SetTableLayoutPanelStyles()
        {
            // Set the styles for the TableLayoutPanel (e.g., row and column styles)
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / tableLayoutPanel.RowCount));
            }

            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tableLayoutPanel.ColumnCount));
            }
        }

        protected override bool CheckForWin(int row, int col)
        {
            // Check if there is a winning combination after a chip is placed
            return CheckDirection(row, col, 0, 1) ||    // Check horizontal
                   CheckDirection(row, col, 1, 0) ||    // Check vertical
                   CheckDirection(row, col, 1, 1) ||    // Check diagonal (up-right)
                   CheckDirection(row, col, 1, -1);     // Check diagonal (up-left)
        }

        protected override bool CheckDirection(int row, int col, int rowIncrement, int colIncrement)
        {
            // Check for a winning combination in a specified direction
            int playerChip = (int)currentPlayer;

            for (int i = -3; i <= 3; i++)
            {
                int newRow = row + i * rowIncrement;
                int newCol = col + i * colIncrement;

                if (IsInBounds(newRow, newCol) && board[newRow, newCol] == playerChip)
                {
                    int count = 0;

                    // Check in both directions
                    for (int j = -3; j <= 3; j++)
                    {
                        int checkRow = row + j * rowIncrement;
                        int checkCol = col + j * colIncrement;

                        if (IsInBounds(checkRow, checkCol) && board[checkRow, checkCol] == playerChip)
                        {
                            count++;

                            if (count == 4)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                }
            }

            return false;
        }




        protected override bool IsInBounds(int row, int col)
        {
            // Check if a specified position is within the bounds of the game board
            return row >= 0 && row < Rows && col >= 0 && col < Columns;
        }

        protected override void IncrementPlayerScore()
        {
            // Increment the score for the current player and update the display
            playerScores[currentPlayer]++;
            label.Text = $"SCORE: {playerName1} - {playerScores[Player.Player1]} : {playerName2} - {playerScores[Player.Player2]}";

            if (playerScores[Player.Player1] >= 2 || playerScores[Player.Player2] >= 2)
            {
                MessageBox.Show($" {currentPlayer} wins!");
                ResetBoard();
            }
            else
            {
                ResetBoard();
            }
        }

        protected override void ResetBoard()
        {
            // Reset the game board to its initial state
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    board[row, col] = 0;
                    Button btn = (Button)tableLayoutPanel.GetControlFromPosition(col, row);
                    btn.BackColor = SystemColors.Control;
                    btn.Enabled = true;
                }
            }
            currentPlayer = Player.Player1;
        }
    }
}
