using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace DelTempFiles
{
    internal class Program : ServiceBase
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        public static void Main(string[] args)
        {

            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);
            // Grabs the appdata and windows temp folder
            string TEMP_APPDATA = Environment.GetEnvironmentVariable("TEMP");
            string TEMP_WINDOWS = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
            // Gets the appdata and windows temp files
            string[] TEMP_APPDATA_FILES = Directory.GetFiles(TEMP_APPDATA);
            string[] TEMP_WINDOWS_FILES = Directory.GetFiles(TEMP_WINDOWS);
            // Gets the appdata and windows temp folders
            string TEMP_APPDATA_FOLDERS = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");
            string TEMP_WINDOWS_FOLDERS = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");

            foreach (string file in TEMP_APPDATA_FILES)
            {
                try
                {
                    // deletes temp files, delete temp folders, and if it can't catch an error and continue running the program
                    File.Delete(file);
                    Directory.Delete(TEMP_APPDATA_FOLDERS);
                }
                catch (IOException ex)
                {
                    // Do Nothing
                }
            }

            foreach (string file in TEMP_WINDOWS_FILES)
            {
                try
                {
                    // deletes temp files and if it can't catch an error and continue running the program
                    File.Delete(file);
                    Directory.Delete(TEMP_WINDOWS_FOLDERS);
                    Console.WriteLine($"Deleted file: {file}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Skipped file: {file} ({ex.Message})");
                }
            }
            /*
            // used for debugging and making sure the program goes to the right folder
            Console.WriteLine("TEMP FOLDER: " + TEMP_APPDATA);
            Console.WriteLine("WINDOWS TEMP FOLDER: " + TEMP_WINDOWS);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            */
        }
    }
}
