using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.IO;
using Ting;


namespace Wpf_app
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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


        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                NotifyPropertyChanged(nameof(FilePath));
            }
        }

        private string clock;
        public string Clock
        {
            get { return clock; }
            set
            {
                clock = value;
                NotifyPropertyChanged(nameof(Clock));
            }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                NotifyPropertyChanged(nameof(Count));
            }
        }

        private ObservableCollection<string> complete_words;
        public ObservableCollection<string> Complete_words
        {
            get { return complete_words; }
            set
            {
                complete_words = value;
                NotifyPropertyChanged(nameof(Complete_words));
            }
        }
        public double change_progessbar()
        {
            progressBar.Value++;
            return progressBar.Value;
        }

        private void Run_button_Click(object sender, RoutedEventArgs e)
        {

            var watch = Stopwatch.StartNew();

            _5W5L.bits(filePath, Text);

            watch.Stop();
            Clock = watch.Elapsed.ToString();
            Count = Text.Count();
            Complete_words = Text;

        }

        private void Fil_button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == true)
            {
                FilePath = ofd.FileName;
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

        private void progressBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}