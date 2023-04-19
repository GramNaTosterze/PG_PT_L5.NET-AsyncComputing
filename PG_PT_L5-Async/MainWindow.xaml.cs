using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PG_PT_L5_Async
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NewtonSymbol _newtonSymbol;
        public MainWindow()
        {
            InitializeComponent();
            _newtonSymbol = new NewtonSymbol(5, 12);
            K.Text = _newtonSymbol.K.ToString();
            N.Text = _newtonSymbol.N.ToString();
        }
        private void UpdateNewtonSymbol()
        {
            _newtonSymbol.N = Int32.Parse(N.Text);
            _newtonSymbol.K = Int32.Parse(K.Text);
        }
        public UInt64 Fib(int n, BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (n <= 0)
                return 0;

            if (worker.CancellationPending)
                e.Cancel = true;
            else
            {
                UInt64[] fib = new UInt64[n];
                fib[0] = 1;
                fib[1] = 1;
                for (int i = 2; i < n; i++)
                {
                    fib[i] = fib[i - 2] + fib[i - 1];

                    int percentage = (int) ((float)i / n * 100);
                    worker.ReportProgress(percentage);
                    Thread.Sleep(20);
                }
                return fib[n - 1];
            }
            return 0;
            
        }
        private void Fibonacci_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                worker.WorkerReportsProgress = true;
                e.Result = Fib((int)e.Argument, worker, e);
            }
        }
        private void Fibonacci_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = (int)e.ProgressPercentage;
        }
        private void FIbonacci_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FibonacciTextBlock.Text = e.Result.ToString();
        }

        // Clicks
        private void CalculateUsingTasks_Click(object sender, RoutedEventArgs e)
        {
            UpdateNewtonSymbol();
            TasksTextBlock.Text = _newtonSymbol.CalculateUsingTasks().ToString();
        }
        private void CalculateUsingDelegates_Click(object sender, RoutedEventArgs e)
        {
            UpdateNewtonSymbol();
            DelegatesTextBlock.Text = _newtonSymbol.CalculateUsingDelegates().ToString();
        }
        private void CalculateUsingAsyncAwait_Click(object sender, RoutedEventArgs e)
        {
            UpdateNewtonSymbol();
            AsyncTextBlock.Text = _newtonSymbol.CalculateUsingAsyncAwait().Result.ToString();
        }
        private void CalculateFibonnaci_Click(object sender, RoutedEventArgs e)
        {
            int i;
            if (!Int32.TryParse(ITextBox.Text, out i) || i <= 0)
                return;
            
            BackgroundWorker fibonacciWorker = new BackgroundWorker();
            fibonacciWorker.DoWork += Fibonacci_DoWork;
            fibonacciWorker.RunWorkerCompleted += FIbonacci_RunWorkerCompleted;
            fibonacciWorker.ProgressChanged += Fibonacci_ProgressChanged;
            ProgressBar.Value = 0;
            fibonacciWorker.RunWorkerAsync(i);

        }
        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dialog.SelectedPath);
                GZip.Compress(directoryInfo);
            }
        }
        private void Decompress_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dialog.SelectedPath);
                GZip.Decompress(directoryInfo);
            }
        }
        private void DNS_Click(object sender, RoutedEventArgs e)
        {
            DnsTextBox.Text = "";
            List<(string, string)> dnsList = DNS.GenerateIPs();
            foreach ((string, string) dns in dnsList)
            {
                DnsTextBox.Text += $"{dns.Item1} => {dns.Item2} \n";
            }
        }
    }
}
