using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Media;

namespace ModifiedConnect4
{

    internal class PlayerVsPlayerTraditional : Form
    {
        private string playerName1;
        private string playerName2;

        private const int rows = 6;
        private const int columns = 7;

        private int[,] board = new int[rows, columns];
        private int currentPlayer = 1;

        private int player1Score = 0;
        private int player2Score = 0;
        private int player1Win = 0;
        private int player2Win = 0;

        private enum Player
        {
            None = 0,
            Player1 = 1,
            Player2 = 2,
        }


        //declaring the forms
        private TableLayoutPanel tableLayoutPanel;
        private Button[,] buttons;
        private Label label;
        private Label playerScoreLabel;
        private Label playerScoreNameLabel;
        private Options options;

        private Options gameOptions = new Options();
        private SoundPlayer chipDropSound = new SoundPlayer("Pop_Sound_Effect.wav");


        public PlayerVsPlayerTraditional(string[] playerName, Options options)
        {
            this.playerName1 = playerName[0];
            this.playerName2 = playerName[1];
            this.options = options;
            MainFrame();
            CreatingBoard();
            ReturnToMainMenuBtn();
            SaveScoreBtn();
            ScoreDisplay(playerName);
        }
        //MAIN FRAME
        public void MainFrame()
        {
            Text = "Connect 4 Game";
            ClientSize = new Size(720, 720); // Size of the form
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.MidnightBlue;
            Focus();
        }

        //Function for RETURNING TO MAIN MENU 
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
        //RETURN TO MAIN MENU BUTTON
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
        //Save Score
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
            try
            {            
                //dir for the exe
                
                string filePath = "PlayerScore.txt";

                
                using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    
                    writer.WriteLine($"{playerName1},{player1Score},{player1Win}");

                    
                    writer.WriteLine($"{playerName2},{player2Score},{player2Win}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving scores: {ex.Message}");
            }
        }
    


    //CREATING LABEL FOR SCORE
    public void ScoreDisplay(string[] playerName)
        {

            label = new Label
            {
                Text = "SCORE",
                Font = new Font("Rockwell", 28, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(725, 30),

            };

            playerScoreLabel = new Label
            {
                Text = $"{player1Score} : {player2Score}",
                Font = new Font("Rockwell", 18, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(725, 30),
            };

            playerScoreNameLabel = new Label
            {
                
                Text = $"{playerName1} : {playerName2}",
                Font = new Font("Rockwell", 14, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(725, 30),
            };
            Controls.Add(label);
            Controls.Add(playerScoreNameLabel);
            Controls.Add(playerScoreLabel);
        }
                      

        //Function for Reseeting the Score
        private void ResetScore()
        {
            player1Score = 0;
            player2Score = 0;

            playerScoreLabel.Text = $"{player1Score} : {player2Score}";
        }

        //FUNCTION for Updating the Score

        private void UpdateScore()
        {
            if (currentPlayer == (int)Player.Player1)
            {
                player1Score++;
                player1Win++;
            }
            else if (currentPlayer == (int)Player.Player2)
            {
                player2Score++;
                player2Win++;
            }

            playerScoreLabel.Text = $"{player1Score} : {player2Score}";

            if (player1Score >= 2 && player2Score == 0)
            {
                MessageBox.Show($"{playerName1} wins!");
                ResetScore();
            }
            else if (player2Score >= 2 && player1Score == 0)
            {
                MessageBox.Show($"{playerName2} wins!");
                ResetScore();
            }
        }
        //function for CREATING BOARD
        private void CreatingBoard()
        {
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Bottom;

            tableLayoutPanel.Size = new Size(0, 500);
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel.RowCount = rows;
            tableLayoutPanel.ColumnCount = columns;

            //applying BUTTONS to tablepanel
            buttons = new Button[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    buttons[row, col] = new Button();
                    buttons[row, col].Dock = DockStyle.Fill;
                    buttons[row, col].Tag = col; // Store column index in the button's Tag property
                    buttons[row, col].Click += DropChip;

                    SetButtonAppearance(buttons[row, col]);
                    tableLayoutPanel.Controls.Add(buttons[row, col], col, row);
                }

            }
            SetTableLayoutPanelStyles();
            Controls.Add(tableLayoutPanel);

        }
        //Function for the DESIGN of the BUTTON
        private void SetButtonAppearance(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.Size = new Size(50, 50);
            //btn.Paint += DrawCircle; 
        }

        //ROWS AND COLUMNS for the TableLayout
        private void SetTableLayoutPanelStyles()
        {
            // Set equal RowStyles
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / tableLayoutPanel.RowCount));
            }

            // Set equal ColumnStyles
            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tableLayoutPanel.ColumnCount));
            }

            foreach (Control control in tableLayoutPanel.Controls)
            {
                if (control is Button)
                {
                    Button btn = (Button)control;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = Color.White;
                    btn.Size = new Size(50, 50);
                }
            }
        }
        //EVENTS
        private void DropChip(object sender, EventArgs e)
        {
            if (IsBoardFull())
            {
                MessageBox.Show("The board is full. Resetting the board.");
                ResetBoard();
                return;
            }
            Button btn = (Button)sender;
            int col = (int)btn.Tag;
            int row = GetAvailableRow(col);
            chipDropSound.Play();

            UpdateBoardAndUI(col);  // Provide both row and col parameters

            if (CheckForWin(row, col))
            {
                UpdateScore();
                ResetBoard();
            }
            else
            {
                currentPlayer = (currentPlayer == (int)Player.Player1) ? (int)Player.Player2 : (int)Player.Player1;
            }

        }
        // Function to check if the entire board is full
        private bool IsBoardFull()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false; //If there is an empty cell, the board is not full
                    }
                }
            }
            return true; //All cells are filled, the board is full
        }

        //FUNCITON for updating the Board after playing
        private void UpdateBoardAndUI(int col)
        {
            int row = GetAvailableRow(col);

            if (row != -1)
            {
                board[row, col] = currentPlayer;
                buttons[row, col].BackColor = (currentPlayer == (int)Player.Player1) ? Color.Red : Color.Yellow;

                // disable the button if the column is now full
                if (row == 0)
                {
                    buttons[row, col].Enabled = false;
                }
            }
        }



        //FUNCTION for checking if there is still available row
        private int GetAvailableRow(int col)
        {
            for (int row = rows - 1; row >= 0; row--)
            {
                if (board[row, col] == 0)
                    return row;
            }
            return -1; // Column is full
        }

        private bool CheckForWin(int row, int col)
        {
            // Check for horizontal win
            for (int c = 0; c <= columns - 4; c++)
            {
                if (board[row, c] == currentPlayer &&
                    board[row, c + 1] == currentPlayer &&
                    board[row, c + 2] == currentPlayer &&
                    board[row, c + 3] == currentPlayer)
                {
                    UpdateScore();
                    ResetBoard();
                    return true;
                }
            }

            // Check for vertical win
            for (int r = 0; r <= rows - 4; r++)
            {
                if (board[r, col] == currentPlayer &&
                    board[r + 1, col] == currentPlayer &&
                    board[r + 2, col] == currentPlayer &&
                    board[r + 3, col] == currentPlayer)
                {
                    //UpdateScore();
                    ResetBoard();
                    return true;
                }
            }

            // Check for diagonal (up-right) win
            for (int r = 0; r <= rows - 4; r++)
            {
                for (int c = 0; c <= columns - 4; c++)
                {
                    if (board[r, c] == currentPlayer &&
                        board[r + 1, c + 1] == currentPlayer &&
                        board[r + 2, c + 2] == currentPlayer &&
                        board[r + 3, c + 3] == currentPlayer)
                    {
                       // UpdateScore();
                        ResetBoard();
                        return true;
                    }
                }
            }

            // Check for diagonal (up-left) win
            for (int r = 0; r <= rows - 4; r++)
            {
                for (int c = 3; c < columns; c++)
                {
                    if (board[r, c] == currentPlayer &&
                        board[r + 1, c - 1] == currentPlayer &&
                        board[r + 2, c - 2] == currentPlayer &&
                        board[r + 3, c - 3] == currentPlayer)
                    {
                       // UpdateScore();
                        ResetBoard();
                        return true;
                    }
                }
            }

            return false;
        }


        //FUNCTION for RESETTING THE BOARD  
        private void ResetBoard()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    board[row, col] = 0;
                    Button btn = (Button)tableLayoutPanel.GetControlFromPosition(col, row);
                    btn.BackColor = SystemColors.Control;
                }
            }
            currentPlayer = 1;
        }


    }
}
