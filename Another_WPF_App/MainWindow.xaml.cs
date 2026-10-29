using Another_5L5W;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Another_WPF_App
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // ObservableCollection giver automatisk besked til WPF når der bliver tilføjet eller fjernet noget
        public ObservableCollection<string> Text { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string clock = string.Empty;
        public string Clock
        {
            get { return clock; }
            set
            {
                clock = value;
                NotifyPropertyChanged(nameof(Clock));
            }
        }

        private string filePath = string.Empty;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                NotifyPropertyChanged(nameof(FilePath));
            }
        }

        private string status = string.Empty;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyPropertyChanged(nameof(Status));
            }
        }

        private int _wordsLoaded;
        private int _progress;

        private async void Run_button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilePath) || !File.Exists(FilePath))
            {
                Status = "Select a valid file";
                return;
            }
            if (Words <= 0)
            {
                Status = "Select a valid number of Words";
                return;
            }
            if (Letters <= 0)
            {
                Status = "Select a valid number of Letters";
                return;
            }

            Run_button.IsEnabled = false;
            Fil_button.IsEnabled = false;
            FilePathTextBox.IsEnabled = false;
            Download_button.IsEnabled = false;
            WordsTextBox.IsEnabled = false;
            LettersTextBox.IsEnabled = false;
            Text.Clear();
            Clock = string.Empty;
            var stopWatch = Stopwatch.StartNew();

            // await Task.Run() bruges til at køre arbejde på en baggrundstråd, så WPF APP ikke fryser.
            var result = await Task.Run(() =>
            {
                var solutionFinder = new SolutionFinder(Words, Letters);
                var wordsLoaded = solutionFinder.LoadWords(FilePath);
                Status = "Loading your words...  " + "Words Found: " + wordsLoaded;
                solutionFinder.FoundSolution += FoundSolutionWriter;
                solutionFinder.Progessbar += ProgressBar;
                _wordsLoaded = wordsLoaded;
                var solutions = solutionFinder.FindSolutions();
                return (wordsLoaded, solutions);
            });

            _wordsLoaded = result.wordsLoaded;
            Status = $"Found {result.solutions.Length} solutions.";

            stopWatch.Stop();
            Clock = stopWatch.Elapsed.ToString(@"mm\:ss\.fff");
            progressBar.Value = _wordsLoaded;
            Run_button.IsEnabled = true;
            Fil_button.IsEnabled = true;
            FilePathTextBox.IsEnabled = true;
            Download_button.IsEnabled = true;
            WordsTextBox.IsEnabled = true;
            LettersTextBox.IsEnabled = true;
        }

        private void ProgressBar(object? sender, int e)
        {
            _progress = e;
            Dispatcher.Invoke(() =>
            {
                progressBar.Maximum = Math.Max(_wordsLoaded, 1);
                progressBar.Value = Math.Min(_progress, progressBar.Maximum);
            });
        }

        private void FoundSolutionWriter(object? sender, string e)
        {
            //  Dispatcher.Invoke() søger for at alt er up to date når også slev om det køre på en anden tråd
            Dispatcher.Invoke(() => Text.Add(e));
        }

        private void Fil_button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == true)
            {
                FilePath = ofd.FileName;
                FilePathTextBox.Text = FilePath;
            }
        }

        private void Download_button_Click(object sender, RoutedEventArgs e)
        {
            if (Text.Count == 0) return;
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Result";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                File.WriteAllLines(dialog.FileName, Text);
            }
        }

        private int Words;
        private int Letters;

        private void WordsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(WordsTextBox.Text, out Words);
        }

        private void LettersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(LettersTextBox.Text, out Letters);
        }
    }
}