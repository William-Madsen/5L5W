using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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

        private void Run_button_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            _5W5L.bits(filePath, Text);

            watch.Stop();
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

        }

        private void progressBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

   
    }
}