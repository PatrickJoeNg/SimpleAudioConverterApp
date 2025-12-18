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
        public MainWindow()
        {
            InitializeComponent();

            //Check if ffmpeg is available
            if (!File.Exists("ffmpeg.exe"))
            {
               MessageBox.Show("ffmpeg.exe not found. Please make sure ffmpeg is in the same directory as this application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FileFieldCheck()
        {
            if (targetSongPath == null || targetOutputPath == null)
            {
                MessageBox.Show("Please select a song and output path before converting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Convert_Click(object sender, RoutedEventArgs e)
        {
            FileFieldCheck();
        }

        private void MenuItem_SelectFile_Click(object sender, RoutedEventArgs e)
        {
            SelectTargetSong();
        }

        private void ConvertSongBtn_Click(object sender, RoutedEventArgs e)
        {
            FileFieldCheck();
        }
        private void SelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectTargetSong();
        }
    }
}