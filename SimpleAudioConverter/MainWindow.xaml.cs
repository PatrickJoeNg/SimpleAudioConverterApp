using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
namespace SimpleAudioConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] audioTypes = [".mp3",".flac"];

        string? targetSongPath;
        string? targetOutputPath;
        string? convertedFolderPath;

        public MainWindow()
        {
            InitializeComponent();
            CheckForFfmpeg();
            CreateConvertedFolder();
        }

        private static void CheckForFfmpeg()
        {
            //Check if ffmpeg is available
            if (!File.Exists("ffmpeg.exe"))
            {
                MessageBox.Show("ffmpeg.exe not found. Please make sure ffmpeg is in the same directory as this application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool FileFieldCheck()
        {
            return targetOutputPath == null || targetSongPath == null ? true : false;
        }
        private void SelectTargetSong()
        {
            OpenFileDialog dlg = new();
            dlg.Filter = "Audio Files|*.flac";
            bool? result = dlg.ShowDialog();

            if(result == true)
            {
                SongSelectionLabel.Content = dlg.SafeFileName;
            }
            targetSongPath = dlg.FileName;
            targetOutputPath = System.IO.Path.ChangeExtension(dlg.SafeFileName, ".mp3");
        }

        private void ConvertSongFile(string targetSong, string outputSong)
        {
            CheckForFfmpeg();
            ProcessStartInfo startInfo = new();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "ffmpeg.exe";
            startInfo.Arguments = $" -i \"{targetSong}\" -b:a 192k \"{convertedFolderPath}\\{outputSong}\"";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    if(exeProcess.ExitCode == 0)
                    {
                        MessageBox.Show("File converted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch(System.Exception ex)
            {
                MessageBox.Show($"An error occurred while converting the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CreateConvertedFolder()
        {
            //creates folder named Converted in the current directory
            if (!Directory.Exists("Converted"))
            {
                MessageBox.Show("No folder for converted songs. Creating...","Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                Directory.CreateDirectory("Converted");
            }
            if(Directory.Exists("Converted"))
            {
                //MessageBox.Show("Converted folder exists!\nSetting as current folder path.","Coverted Folder Check", MessageBoxButton.OK, MessageBoxImage.Information);
                convertedFolderPath = System.IO.Path.GetFullPath("Converted");
            }
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Convert_Click(object sender, RoutedEventArgs e)
        {
                if (FileFieldCheck())
                {
                    MessageBox.Show("Please select a song and output path before converting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            ConvertSongFile(targetSongPath, targetOutputPath);
        }

        private void MenuItem_SelectFile_Click(object sender, RoutedEventArgs e)
        {
            SelectTargetSong();
        }

        private void ConvertSongBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FileFieldCheck())
            {
                MessageBox.Show("Please select a song and output path before converting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ConvertSongFile(targetSongPath, targetOutputPath);
        }
        private void SelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectTargetSong();
        }

        private void SelectFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AboutThisMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Program made by Patrick Ng.\nCredits to FFmpeg team, gyandev for precompiled.","About This",MessageBoxButton.OK);

        }

        private void MenuItem_Get_FFmpeg_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = "https://www.gyan.dev/ffmpeg/builds/#release-builds",
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }

        private void MenuItem_OpenConvertedFolder_Click(object sender, RoutedEventArgs e)
        {
            CreateConvertedFolder();
            ProcessStartInfo startInfo = new()
            {
                FileName = convertedFolderPath,
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(startInfo);
        }
    }
}