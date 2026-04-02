//Group Members - Grefaldo, Sophia Nicole, Lelis Rosette Michaela, Usisa Mahrcus Leanard
//Sectio: B55
//Date: 11/22/2023
//Description: Modified Connect4 (Just Like the Traditional Connect4Game but the player will earn 2 points to win the game
//there is also a free mode where the players are free to place the chips anywhere in the board)


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;

namespace ModifiedConnect4
{
    internal class MainMenu : Form
    {
        // creating properties
        public Form startForm;
        public Form form3;
        public Form form4;
        public Label connect4label;
        public Label label2;
        public Button startBtn;
        public Button leaderboardBtn;
        public Button optionsBtn;
        public Button howtoPlayBtn;
        public Button exitBtn;

        public MainMenu()
        {
            MainFrame();
            Connect4Label();
            Start();
            Leaderboards();
            Options();
            HowtoPlay();
            Exit();

        }

        //MAIN FraME
        public void MainFrame()
        {
            Text = "Connect 4 Game";
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

        //Start Button
        public void Start()
        {
            startBtn = new Button();
            startBtn.Size = new Size(300, 30);
            startBtn.BackColor = Color.Red;
            startBtn.Text = "Start";
            startBtn.Font = new Font("Rockwell", 20, FontStyle.Bold);
            startBtn.Dock = DockStyle.None;
            startBtn.Location = new Point(200, 100);
            this.Controls.Add(startBtn);
            startBtn.Click += new EventHandler(StartClick);
        }
        //Leaderboards button
        public void Leaderboards()
        {
            leaderboardBtn = new Button();
            leaderboardBtn.Size = new Size(300, 30);
            leaderboardBtn.BackColor = Color.Yellow;
            leaderboardBtn.Font = new Font("Rockwell", 20, FontStyle.Bold);
            leaderboardBtn.Text = "Leaderboards";
            leaderboardBtn.Location = new Point(200, 150);
            this.Controls.Add(leaderboardBtn);
            leaderboardBtn.Click += new EventHandler(LeaderboardsClick);
        }
        //Options Button
        public void Options()
        {
            optionsBtn = new Button();
            optionsBtn.Size = new Size(300, 30);
            optionsBtn.BackColor = Color.Red;
            optionsBtn.Font = new Font("Rockwell", 20, FontStyle.Bold);
            optionsBtn.Text = "Options";
            optionsBtn.Location = new Point(200, 200);
            this.Controls.Add(optionsBtn);
            optionsBtn.Click += new EventHandler(OptionsClick);
        }
        //How to Play Button
        public void HowtoPlay()
        {
            howtoPlayBtn = new Button();
            howtoPlayBtn.Size = new Size(300, 30);
            howtoPlayBtn.BackColor = Color.Yellow;
            howtoPlayBtn.Font = new Font("Rockwell", 20, FontStyle.Bold);
            howtoPlayBtn.Text = "How to Play";
            howtoPlayBtn.Location = new Point(200, 250);
            this.Controls.Add(howtoPlayBtn);
            howtoPlayBtn.Click += new EventHandler(HowtoPlay_Click);
        }
        //Exit
        public void Exit()
        {
            exitBtn = new Button();
            exitBtn.Size = new Size(300, 30);
            exitBtn.BackColor = Color.Red;
            exitBtn.Text = "Exit";
            exitBtn.Font = new Font("Rockwell", 20, FontStyle.Bold);
            exitBtn.Location = new Point(200, 300);
            this.Controls.Add(exitBtn);
            exitBtn.Click += new EventHandler(ExitClick);
        }

        //EVENTS
        // code for each of the button with event 
        private void StartClick(object sender, EventArgs e)
        {

            Start startForm = new Start();
            startForm.Show();
            this.Hide();

        }

        private void LeaderboardsClick(object sender, EventArgs e)
        {
            Leaderboards leaderbaordsForm = new Leaderboards();
            leaderbaordsForm.Show();
            this.Hide();
        }

        private void OptionsClick(object sender, EventArgs e)
        {
            Options option = new Options();
            option.Text = "Options";
            //code for options
            option.Show();
            this.Hide();
        }

        private void HowtoPlay_Click(object sender, EventArgs e)
        {
            Mechanics mechanics = new Mechanics();
            string mechanicsContent = mechanics.HowToPlayData();

            MessageBox.Show(mechanicsContent,"Mechanics",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
            

        private void ExitClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }

        }


    }
}
