using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModifiedConnect4
{
    internal class Program : Form
    {
        static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();
            Application.Run(mainMenu);
        }
    }
}
