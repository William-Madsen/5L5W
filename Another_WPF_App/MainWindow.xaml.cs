using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Another_5W5L_Console;
using Another_5L5W;

namespace Another_WPF_App
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

        private void Run_button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Collect.Main();

            stopWatch.Stop();

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

        }
    }
}