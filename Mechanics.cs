using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModifiedConnect4
{
    internal class Mechanics
    {
        private string mechanicsFilePath { get; set; }



        public Mechanics()
        {

        }
        public string HowToPlayData()
        {
            // dir of the text file
            mechanicsFilePath = "Mechanics.txt";
            try
            {
                using (FileStream mechanicsData = new FileStream(mechanicsFilePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(mechanicsData))
                {
                    List<string> fileContent = new List<string>();

                    
                    while (!reader.EndOfStream)
                    {
                        fileContent.Add(reader.ReadLine());
                    }

                    
                    return string.Join("\n" ,fileContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                
                return $"Error: {ex.Message}";
            }
        }

    }
}



