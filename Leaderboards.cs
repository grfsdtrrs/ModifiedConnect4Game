using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Configuration;

namespace ModifiedConnect4
{
    internal class Leaderboards : Form
    {
        //properties
        private string playerName { get; set; }
        private string playerType { get; set; }

        private TextBox textboxData;
        private Label leaderboardLabel;

        public Leaderboards()
        {
            MainFrame();
            ReturnToMainMenuBtn();
            LeadDisp();
            LeaderBoardLabel();

        }
        //this function controls the main or parent frame

        public void MainFrame()
        {
            Text = "Leaderboards";
            ClientSize = new Size(720, 720); //Size of the form
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        //LABEL FOR LEADERBOARDS
        public void LeaderBoardLabel()
        {

            leaderboardLabel = new Label
            {
                Text = "LEADERBOARDS",
                Font = new Font("SHOWCARD GOTHIC", 28, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSize = false,
                Size = new Size(725, 72),
                BackColor = Color.Gold,

            };

            Controls.Add(leaderboardLabel);

        }
        //BUTTON for MainMenu
        public void ReturnToMainMenuBtn()
        {
            Button returnMenu = new Button();

            returnMenu.Text = "Return to Menu";

            returnMenu.Click += new EventHandler(ReturnClickBtn);
            returnMenu.BackColor = Color.White;
            returnMenu.Font = new Font("Rockwell", 18, FontStyle.Regular);
            returnMenu.FlatStyle = FlatStyle.Popup;
            returnMenu.Dock = DockStyle.Bottom;
            returnMenu.Size = new Size(120, 50);
            this.Controls.Add(returnMenu);

        }
       //if ReturnMenu Button was click
        public void ReturnClickBtn(object sender, EventArgs e)
        {                      
                MainMenu menu = new MainMenu();
                menu.Show();
                this.Hide();            

        }
        //TEXTBOX used to display the contents inside the text file
        public void LeadDisp()
        {
            ////Textbox data settings
            textboxData = new TextBox
            {
                ReadOnly = true,
                Multiline = true,
                TabStop = false,
                BackColor = Color.LightGoldenrodYellow,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new Font("Rockwell", 18),
                Size = new Size(200, 100),
                Location = new Point(100, 100),
            };

            this.Resize += (sender, e) =>
            {
                textboxData.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 150);
            };


            Controls.Add(textboxData);

            LeaderboardsData();
        }

        //Function for Data of the TextFile
        public void LeaderboardsData()
        {
            // dir of the text file 
            string filePath = "PlayerScore.txt";

            try
            {
                
                using (FileStream playerScoreFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(playerScoreFile))
                {
                    List<string> fileContent = new List<string>();

                    // Read all lines from the file
                    while (!reader.EndOfStream)
                    {
                        fileContent.Add(reader.ReadLine());
                    }

                    // Sorting to Descending Order
                    fileContent.Sort((x, y) =>
                    {
                        string[] playerInfoX = x.Split(',');
                        string[] playerInfoY = y.Split(',');

                        // check if the data in the text file has 2 elements (points and win)
                        if (playerInfoX.Length >= 2 && playerInfoY.Length >= 2)
                        {
                            int pointsX = int.Parse(playerInfoX[1].Trim());
                            int pointsY = int.Parse(playerInfoY[1].Trim());

                            return pointsY.CompareTo(pointsX);
                        }
                        return 0;
                    });

                    // uses StringBuilder for inserting the text in a specific location
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine($"{"RANK",-15}{"PLAYER NAME",-26}{"POINTS",-15}{"WINS",15}");

                    // loops to read all the data in the text file
                    for (int i = 0; i < fileContent.Count; i++)
                    {
                        string[] playerInfo = fileContent[i].Split(',');

                        // CHECK if the array has at least 3 elements (Rank, Player Name, Points)
                        if (playerInfo.Length >= 3)
                        {
                            string rank = (i + 1).ToString();
                            string playerName = playerInfo[0].Trim();
                            string points = playerInfo[1].Trim();
                            string wins = playerInfo[2].Trim();

                            // appends to the data in the file(the '-' shows how far the distance to each other)
                            sb.AppendLine($"{"    " + rank,-5}{"\t     " + playerName,-25}{"\t             " + points,-10}{"\t\t       " + wins}");
                        }
                    }

                    // Display the data in the TextBox
                    textboxData.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }



    }


}
