using System.IO;
using System;
using System.Text;
using Microsoft.Graph;
using RSCDemo.Models;
using File = System.IO.File;

namespace RSCDemo.Utils
{
    public class AppState
    {

        public static string GetAppState ()
        {
            String line;
            StringBuilder sb = new StringBuilder();
            try
            {
                string file = @"D:\\hackathon\\teams_channel_buddy\\RSCDemo\\appState.txt";
                //Console.WriteLine("Hello");
                if (File.Exists(file))
                {
                    // Store each line in array of strings
                    string[] lines = File.ReadAllLines(file);

                    foreach (string ln in lines)
                    {
                        if (!String.IsNullOrWhiteSpace(ln))
                        {
                            string[] appState = ln.Split("\t");
                            sb.Append(ln);
                        }
                        
                    }
                       
                }
                UpdateAppState(new string[] { "Test message 1", "Test Message 2" });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
               
            }
            return sb.ToString();
        }

        public static void UpdateAppState(string[] appState)
        {
            string contents = "Hello\nGoodbye";
            StringBuilder content = new StringBuilder();
            for (int i = 0; i < appState.Length; i++)
            {
                content.Append(appState[i]);
                content.Append("\n");
            }
            string file = @"D:\\hackathon\\teams_channel_buddy\\RSCDemo\\appState.txt";
            try
            {
                File.WriteAllText(file, content.ToString());
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
       
    }
}
