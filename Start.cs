
using ModifiedConnect4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ModifiedConnect4
{
    internal class Start : Form
    {
        private Button pvpTraditional;
        private Button pvpFreesytle;
        private Button pvBot;
        private Button returnMenu;


        private Label connect4label;

        private Form pvpTradForm;
        private TextBox player1TextBox;
        private TextBox player2TextBox;

        public Start()
        {

            MainFrame();
            Connect4Label();
           // InputPlayerName();
            PlayerVsPlayerTradBtn();
            PlayerVsPlayerFreeBtn();
            PlayerVsBotBtn();
            ReturnMainMenu();
        }

        public void MainFrame()
        {
            Text = "Start";
            ClientSize = new Size(720, 720); // Size of the form
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.MidnightBlue;
            Focus();
        }

        //LABEL FOR CONNECT4
        public void Connect4Label()
        {
            connect4label = new Label();
            connect4label.Text = "WELCOME TO CONNECT4";
            connect4label.TextAlign = ContentAlignment.MiddleCenter;
            connect4label.Font = new Font("SHOWCARD GOTHIC", 28, FontStyle.Bold);
            connect4label.BackColor = Color.Yellow;
            connect4label.Size = new Size(100, 50);
            connect4label.AutoSize = false;
            connect4label.Dock = DockStyle.Top;
            connect4label.Location = new Point(725, 72);
            this.Controls.Add(connect4label);
        }

        //TEXTBOX FOR GETTING the name of the player
        private string[] InputPlayerName()
        {
            //Textbox for player input
            player1TextBox = new TextBox
            {
                Size = new Size(200, 30),
                Location = new Point(10, 50),
            };

            player2TextBox = new TextBox
            {
                Size = new Size(200, 30),
                Location = new Point(10, 90),
            };
            int width = 300;
            int height = 200;

            //FORM FOR player name
            Form inputname = new Form
            {
                Text = "Player Name",
                Size = new Size(width, height),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,


            };

            //Label for player1 name
            Label greetPlayer1 = new Label
            {
                Text = "Please Input name of Player 1",
                Location = new Point(10, 20),
                AutoSize = true,
            };

            //Label for player2 name
            Label greetPlayer2 = new Label
            {
                Text = "Please Input name of Player 2",
                Location = new Point(10, 70),
                AutoSize = true,
            };


            //Button for OK
            Button okBtn = new Button
            {
                Text = "OK",
                Size = new Size(80, 30),
                Location = new Point(150, 105),
            };

            inputname.Controls.Add(greetPlayer1);            
            inputname.Controls.Add(player1TextBox);
            inputname.Controls.Add(greetPlayer2);
            inputname.Controls.Add(player2TextBox);
            inputname.Controls.Add(okBtn);

            //validation
            //event for OK button
            string[] playerName = new string[2];
            okBtn.Click += (s, ev) =>
            {
                // Check if the player entered a name
                if (string.IsNullOrWhiteSpace(player1TextBox.Text) || string.IsNullOrWhiteSpace(player2TextBox.Text))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    playerName[0] = player1TextBox.Text.Trim();
                    playerName[1] = player2TextBox.Text.Trim();
                    inputname.Close();
                }
            };

            inputname.FormClosing += (s, ev) =>
            {
                // Prevent closing without entering a name
                if (string.IsNullOrWhiteSpace(player1TextBox.Text) || string.IsNullOrWhiteSpace(player2TextBox.Text))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ev.Cancel = true;
                }
            };

            // Disable the close button in the window title bar
            inputname.ControlBox = false;

            inputname.ShowDialog();

            return playerName;
        }
        //STORING the name of the player
        
        


        //Button for PLayer vs Player Traditional
        public void PlayerVsPlayerTradBtn()
        {
            pvpTraditional = new Button
            {
                Size = new Size(350, 30),
                BackColor = Color.Red,
                Text = "Player vs Player (Traditional)",
                Font = new Font("Rockwell", 16, FontStyle.Bold),
                Dock = DockStyle.None,
                Location = new Point(200, 300),
            };
            this.Controls.Add(pvpTraditional);
            pvpTraditional.Click += new EventHandler(PvpTradClick);
        }

        //Button for Player vs Player FREE MODE
        private void PlayerVsPlayerFreeBtn()
        {
            pvpFreesytle = new Button
            {
                Size = new Size(350, 30),
                BackColor = Color.Red,
                Text = "Player vs Player (FREE MODE)",
                Font = new Font("Rockwell", 16, FontStyle.Bold),
                Dock = DockStyle.None,
                Location = new Point(200, 350),
            };
            this.Controls.Add(pvpFreesytle);
            pvpFreesytle.Click += new EventHandler(PvpFreeClick);
        }

        //Button for Player vs Bot
        public void PlayerVsBotBtn()
        {
            pvBot = new Button
            {
                Size = new Size(350, 30),
                BackColor = Color.Red,
                Text = "Player vs Computer",
                Font = new Font("Rockwell", 16, FontStyle.Bold),
                Dock = DockStyle.None,
                Location = new Point(200, 400),
            };
            this.Controls.Add(pvBot);
            pvBot.Click += new EventHandler(PvBotClick);

        }
        //BUTTON for Return Menu
        public void ReturnMainMenu()
        {
            returnMenu = new Button
            {
                Size = new Size(350, 30),
                BackColor = Color.Red,
                Text = "Return to Main Menu",
                Font = new Font("Rockwell", 16, FontStyle.Bold),
                Dock = DockStyle.None,
                Location = new Point(200, 450),
            };
            this.Controls.Add(returnMenu);
            returnMenu.Click += new EventHandler(ReturnMenuClick);
        }


        //
        //Events
        //

        //CLICK for Player vs Player (TRADITIONAL)
        private void PvpTradClick(object sender, EventArgs e)
        {
            string[] playerName = InputPlayerName();

            // Create an instance of Options
            Options options = new Options();

            PlayerVsPlayerTraditional pvpTrad = new PlayerVsPlayerTraditional(playerName, options);
            pvpTrad.Show();
            this.Hide();
        }
        //CLICK for Player vs Player (FREE MODE)
        // CLICK for Player vs Player (FREE MODE)
        private void PvpFreeClick(object sender, EventArgs e)
        {
            string[] playerName = InputPlayerName();

            // Create an instance of Options
            Options options = new Options();

            PlayerVsPlayerFreeMode pvpFreeMode = new PlayerVsPlayerFree(playerName, options);
            pvpFreeMode.Show();
            this.Hide();
        }


        //CLICK for Player vs Computer
        private void PvBotClick(object sender, EventArgs e)
        {
            // Create an instance of Options
            Options options = new Options();

            PlayerVsBot playervsbot = new PlayerVsBot(options);
            playervsbot.Show();
            this.Hide();
        }
        //CLICK for Player vs Computer
        private void ReturnMenuClick(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Hide();
        }
    }
        
}
