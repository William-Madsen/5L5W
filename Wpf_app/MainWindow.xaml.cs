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

        private int _percent = 0;
        public int Percent
        {
            get { return _percent; }
            set { _percent = value; NotifyPropertyChanged("Percent"); }

        }

        private async void Run_button_Click(object sender, RoutedEventArgs e)
        {
            IProgress<Report.how_long> progess = new Progress<Report.how_long>((Report.how_long progess) =>
            {
                progressBar.Value = progess.How_far / progess.total;
            });
            var watch = Stopwatch.StartNew();

            List<string> _Text = new List<string>();
            await Task.Run(() =>
            {
                _5W5L.bits(filePath, _Text, progess);
            });
            for (int i = 0; i < _Text.Count; i++)
            {
                Text.Add(_Text[i]);
            }

            watch.Stop();
            Percent = 100;
            Clock = watch.Elapsed.ToString();
            Count = Text.Count();

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
        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}