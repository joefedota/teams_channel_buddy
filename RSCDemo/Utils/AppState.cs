using System.IO;
using System;
using System.Text;
using Microsoft.Graph;
using RSCDemo.Models;
using File = System.IO.File;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using RSCDemo.Helpers;

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
                AppStateModel appState1 = new AppStateModel();
                appState1.ChannelId = 1;
                appState1.Summary = "Test Summary";
                appState1.ActionItem = "Test Action Item";
                //appState1.LastMessage = new CustomMessage("Test Message", DateTimeOffset.Now, string.Empty, string.Empty);

                AppStateModel appState2 = new AppStateModel();
                appState2.ChannelId = 2;
                appState2.Summary = "Test Summary 2";
                appState2.ActionItem = "Test Action Item 2";
                //appState2.LastMessage = new CustomMessage("Test Message 2", DateTimeOffset.Now, string.Empty, string.Empty);

                UpdateAppStateUsingJson(new AppStateModel[] { appState1, appState2}); 
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

        public static void UpdateAppStateUsingJson(AppStateModel[] appStates)
        {
            string file = @"D:\\hackathon\\teams_channel_buddy\\RSCDemo\\appState.txt";
            for (int i = 0; i < appStates.Length; i++)
            {
               GraphHelper.WriteToJsonFile(file, appStates[i], true);
            }

        }
        
       
    }
}
