using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace ModifiedConnect4
{
    public partial class Options : Form
    {
        // Properties to store selected options
        public Color ChipColorPlayer1 { get; set; }
        public Color ChipColorPlayer2 { get; set; }
        public bool EnableSounds { get; set; }
        public string SelectedSoundFile { get; set; }

       

        private ComboBox cmbChipColorPlayer1 = new ComboBox();
        private ComboBox cmbChipColorPlayer2 = new ComboBox();
        private Label lblPlayer1Color;
        private Label lblPlayer2Color;
        private CheckBox chkEnableSounds;
        private Button btnChooseSound;

        // Sound player for button click sound
        private SoundPlayer buttonClickSoundPlayer;



        public Options()
        {
            InitializeComponent();
            InitializeSoundPlayer();
            InitializeChipColorComboBox();
            ReturnToMainMenuBtn();
            SaveBtn();
        }

        private void InitializeComponent()
        {
            this.lblPlayer1Color = new Label();
            this.lblPlayer2Color = new Label();
            this.chkEnableSounds = new CheckBox();
            this.btnChooseSound = new Button();
            this.SuspendLayout();

            // Set up form properties
            this.Text = "Connect 4 Options";
            this.Size = new Size(400, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize controls
            this.lblPlayer1Color = new Label
            {
                Text = "Player 1 Chip Color:",
                Location = new Point(20, 30)
            };

            this.lblPlayer2Color = new Label
            {
                Text = "Player 2 Chip Color:",
                Location = new Point(20, 70)
            };

            this.chkEnableSounds = new CheckBox
            {
                Text = "Enable Sounds",
                Location = new Point(20, 110),
                Checked = true
            };

            // Add controls to the form
            this.Controls.Add(lblPlayer1Color);
            
            this.Controls.Add(lblPlayer2Color);
           
            this.Controls.Add(chkEnableSounds);

            this.ResumeLayout(false);
        }

        // Initialize the sound player and set default values
        private void InitializeSoundPlayer()
        {
            // Set the default sound file path or change it to your default sound
            SelectedSoundFile = "C:\\Users\\chris\\Downloads\\Connect4_FreeStyle_Updated5\\Connect4_FreeStyle\\bin\\Debug\\Pop_Sound_Effect.wav";
            buttonClickSoundPlayer = new SoundPlayer(SelectedSoundFile);
        }

        // Initialize the chip color ComboBox with color options
        private void InitializeChipColorComboBox()
        {
            // Create ComboBox for Player 1
            cmbChipColorPlayer1 = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(150, 30),
                Size = new Size(100, 20)
            };

            // Add color options to the ComboBox for Player 1
            cmbChipColorPlayer1.Items.Add("Red");
            cmbChipColorPlayer1.Items.Add("Yellow");
            cmbChipColorPlayer1.Items.Add("Green");
            cmbChipColorPlayer1.Items.Add("Orange");
            cmbChipColorPlayer1.Items.Add("Purple");

            // Set the default selection
            cmbChipColorPlayer1.SelectedIndex = 0;

            // Create ComboBox for Player 2
            cmbChipColorPlayer2 = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(150, 70),
                Size = new Size(100, 20)
            };

            // Add color options to the ComboBox for Player 2
            cmbChipColorPlayer2.Items.Add("Red");
            cmbChipColorPlayer2.Items.Add("Yellow");
            cmbChipColorPlayer2.Items.Add("Green");
            cmbChipColorPlayer2.Items.Add("Orange");
            cmbChipColorPlayer2.Items.Add("Purple");

            // Set the default selection
            cmbChipColorPlayer2.SelectedIndex = 0;

            // Add ComboBoxes to the form
            this.Controls.Add(cmbChipColorPlayer1);
            this.Controls.Add(cmbChipColorPlayer2);
        }

        // Event handler for confirming options
        private void btnOK_Click(object sender, EventArgs e)
        {
            EnableSounds = chkEnableSounds.Checked;

            // Save the selected sound file path
            SelectedSoundFile = btnChooseSound.Tag?.ToString();

            // Save the selected chip colors from the ComboBoxes
            string selectedColorNamePlayer1 = cmbChipColorPlayer1.SelectedItem.ToString();
            string selectedColorNamePlayer2 = cmbChipColorPlayer2.SelectedItem.ToString();
            ChipColorPlayer1 = GetColorByName(selectedColorNamePlayer1);
            ChipColorPlayer2 = GetColorByName(selectedColorNamePlayer2);

            // Create an instance of PlayerVsPlayerFree and pass the options
            Options options = new Options
            {
                ChipColorPlayer1 = this.ChipColorPlayer1,
                ChipColorPlayer2 = this.ChipColorPlayer2,
                EnableSounds = this.EnableSounds,
                SelectedSoundFile = this.SelectedSoundFile
            };

            PlayerVsPlayerFree playerVsPlayerFree = new PlayerVsPlayerFree(new string[] { "Player1", "Player2" }, options);

            // Show the PlayerVsPlayerFree form
            playerVsPlayerFree.Show();

            // Close the Options form
            this.Close();
        }

        //Creation for RETURNING TO MAIN MENU BTN
        public void ReturnToMainMenuBtn()
        {
            Button returnMenu = new Button();

            returnMenu.Text = "Return to Menu";

            returnMenu.Click += new EventHandler(ReturnClickBtn);
            returnMenu.BackColor = Color.White;
            returnMenu.Font = new Font("Rockwell", 9, FontStyle.Regular);
            returnMenu.FlatStyle = FlatStyle.Popup;
            returnMenu.Size = new Size(120, 20);
            returnMenu.Location = new Point(250, 160);
            this.Controls.Add(returnMenu);

        }
        //Event (return main menu click)
        public void ReturnClickBtn(object sender, EventArgs e)
        {                     
            MainMenu returnMenu = new MainMenu();
            returnMenu.Show();
            this.Hide();

        }
        //Creation of button for save 
        public void SaveBtn()
        {
            Button saveBtn = new Button();

            saveBtn.Text = "Save";

            saveBtn.Click += new EventHandler(SaveClick);
            saveBtn .BackColor = Color.White;
            saveBtn.Font = new Font("Rockwell", 9, FontStyle.Regular);
            saveBtn.FlatStyle = FlatStyle.Popup;
            saveBtn.Size = new Size(90, 20);
            saveBtn.Location = new Point(150, 160);
            this.Controls.Add(saveBtn);

        }

        public void SaveClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to save the settings?","Warning",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
            
            if (result == DialogResult.OK)
            {
                //save the selected color of the chip
             

                MainMenu returnMenu = new MainMenu();
                returnMenu.Show();
                this.Hide();
            }
            
            

        }


        // Event handler for canceling options
        private void btnCancel_Click(object sender, EventArgs e)
        {
            PlayButtonClickSound();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Event handler for choosing Player 1's chip color
        private void btnPlayer1Color_Click(object sender, EventArgs e)
        {
            // Show color dialog for selecting color
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ChipColorPlayer1 = colorDialog.Color;
                lblPlayer1Color.BackColor = ChipColorPlayer1;
                PlayButtonClickSound();
            }
        }

        // Event handler for choosing Player 2's chip color
        private void btnPlayer2Color_Click(object sender, EventArgs e)
        {
            // Show color dialog for selecting color
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ChipColorPlayer2 = colorDialog.Color;
                lblPlayer2Color.BackColor = ChipColorPlayer2;
                PlayButtonClickSound();
            }
        }

        // Event handler for choosing a custom sound file
        private void btnChooseSound_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Sound Files|*.wav;*.mp3|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the selected sound file path in the Tag property of the button
                btnChooseSound.Tag = openFileDialog.FileName;
                PlayButtonClickSound();
            }
        }

        // Play the button click sound if sounds are enabled
        private void PlayButtonClickSound()
        {
            if (EnableSounds)
            {
                buttonClickSoundPlayer.Play();
            }
        }

        // Get a color by name (for ComboBox selection)
        private Color GetColorByName(string colorName)
        {
            switch (colorName.ToLower())
            {
                case "red":
                    return Color.Red;
                case "yellow":
                    return Color.Yellow;
                case "green":
                    return Color.Green;
                case "orange":
                    return Color.Orange;
                case "purple":
                    return Color.Purple;
                default:
                    return Color.Black; 
            }
        }
        
    }
}
