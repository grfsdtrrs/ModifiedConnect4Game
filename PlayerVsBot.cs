using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace ModifiedConnect4
{
    internal class PlayerVsBot : Form
    {
        private const int rows = 6;
        private const int columns = 7;

        private int[,] board = new int[rows, columns];
        private int currentPlayer = 1;

        private int playerScore = 0;
        private int computerScore = 0;

        //Forms
        private TableLayoutPanel tableLayoutPanel;
        private Button[,] buttons;
        private Label label;
        private Label playerScoreLabel;
        private Label computerScoreLabel;
       // private Options options = new Options(this);
        private SoundPlayer chipDropSound = new SoundPlayer("Pop_Sound_Effect.wav");

        private Options options;

        public Options Options
        {
            get { return options; }
            set { options = value; }
        }

        public PlayerVsBot(Options options)
        {
            this.options = options;
            MainFrame();
            CreatingBoard();
            ReturnToMainMenuBtn();
            ScoreDisplay();
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

            DialogResult result = MessageBox.Show("Returning to Main Menu without finishing the game will not save your data", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result == DialogResult.OK)
            {
                MainMenu menu = new MainMenu();
                menu.Show();
                this.Hide();
            }

        }

        //CREATING LABEL FOR SCORE
        public void ScoreDisplay()
        {
            label = new Label
            {
                Text = "SCORE",
                Font = new Font("Rockwell", 28, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,

                ForeColor = Color.White,
                // BorderStyle = BorderStyle.FixedSingle,
                AutoSize = false,
                Size = new Size(725, 50),


            };

            playerScoreLabel = new Label
            {
                Text = $"{playerScore} : {computerScore}",
                Font = new Font("Rockwell", 18, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(725, 30),
            };

            computerScoreLabel = new Label
            {
                Text = "Player : Computer",
                Font = new Font("Rockwell", 14, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(725, 30),
            };

            Controls.Add(computerScoreLabel);
            Controls.Add(playerScoreLabel);
            Controls.Add(label);
        }

        //FUNCTION for Updating the Score
        private void UpdateScore()
        {
            if (currentPlayer == 1)
            {
                playerScore++;
            }
            else if (currentPlayer == 2)
            {
                computerScore++;                                
            }
            playerScoreLabel.Text = $"{playerScore} : {computerScore}";
            if (playerScore >= 2 || computerScore >= 2)
            {
                MessageBox.Show($" {(playerScore >= 2 ? "player1" : "computer")} wins!");
                ResetScore();        
            }
        }

                       

        // FUNCTION for Resetting the Score
        private void ResetScore()
        {
            playerScore = 0;
            computerScore = 0;
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
        //logic for determining the winner
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
            chipDropSound.Play();
            int row = GetAvailableRow(col);

            if (row != -1)
            {
                UpdateBoardAndUI(row, col);

                if (CheckForWin(row, col))
                {
                    UpdateScore();
                    ResetBoard();
                }
                else
                {
                    currentPlayer = 2;
                    if (currentPlayer == 2)
                    {
                        PerformComputerMove();

                    }
                }
            }
            else
            {
                MessageBox.Show("Column is full. Choose another column.");
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
        private void UpdateBoardAndUI(int row, int col)
        {
            board[row, col] = currentPlayer;
            Options options = new Options();

            // Get the selected chip colors from the options
            Color player1ChipColor = options.ChipColorPlayer1;
            Color player2ChipColor = options.ChipColorPlayer2;

            buttons[row, col].BackColor = (currentPlayer == 1) ? player1ChipColor : player2ChipColor;
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
        //FUNCTION for checking winning conditions
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
                    ResetBoard();
                    return true;
                }
            }

            // Check for diagonal (up-right) "\"
            for (int r = 0; r <= rows - 4; r++)
            {
                for (int c = 0; c <= columns - 4; c++)
                {
                    if (board[r, c] == currentPlayer &&
                        board[r + 1, c + 1] == currentPlayer &&
                        board[r + 2, c + 2] == currentPlayer &&
                        board[r + 3, c + 3] == currentPlayer)
                    {
                        ResetBoard();
                        return true;
                    }
                }
            }

            // Check for diagonal (up-left) "/"
            for (int r = 0; r <= rows - 4; r++)
            {
                for (int c = 3; c < columns; c++)
                {
                    if (board[r, c] == currentPlayer &&
                        board[r + 1, c - 1] == currentPlayer &&
                        board[r + 2, c - 2] == currentPlayer &&
                        board[r + 3, c - 3] == currentPlayer)
                    {
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

        //FUNCTION for ComputerPlay
        private async void PerformComputerMove()
        {

            await Task.Delay(900);
            Random random = new Random();
            int computerCol = random.Next(columns);

            int computerRow = GetAvailableRow(computerCol);
            chipDropSound.Play();
            if (computerRow != -1)
            {
                UpdateBoardAndUI(computerRow, computerCol);
                if (CheckForWin(computerRow, computerCol))
                {

                    MessageBox.Show($"Computer wins!");

                    ResetBoard();
                }
                else
                {
                    currentPlayer = 1; // Switch back to the player after the computer's move
                }
            }
        }

    }
}

