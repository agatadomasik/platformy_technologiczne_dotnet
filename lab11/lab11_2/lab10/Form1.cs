using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FibonacciAndCompression
{
    public partial class Form1 : Form
    {
        private BackgroundWorker fibonacciWorker = new BackgroundWorker();
        private CancellationTokenSource cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();

            fibonacciWorker.WorkerReportsProgress = true;
            fibonacciWorker.DoWork += FibonacciWorker_DoWork;
            fibonacciWorker.ProgressChanged += FibonacciWorker_ProgressChanged;
            fibonacciWorker.RunWorkerCompleted += FibonacciWorker_RunWorkerCompleted;
        }

        private void FibonacciWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int n = (int)e.Argument;
            int fibPrev = 0;
            int fibCurr = 1;

            for (int i = 2; i <= n; i++)
            {
                int fibNext = fibPrev + fibCurr;
                fibPrev = fibCurr;
                fibCurr = fibNext;

                Thread.Sleep(5);

                int progressPercentage = (int)((double)i / n * 100);
                fibonacciWorker.ReportProgress(progressPercentage, fibCurr);
            }

            e.Result = fibCurr;
        }

        private void FibonacciWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void FibonacciWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCalculate.Enabled = true;

            if (!e.Cancelled && e.Error == null)
            {
                int fibonacciResult = (int)e.Result;
                MessageBox.Show($"Result: {fibonacciResult}");
            }
            else if (e.Error != null)
            {
                MessageBox.Show($"Error: {e.Error.Message}");
            }
        }

        private async void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!fibonacciWorker.IsBusy)
            {
                if (int.TryParse(txtInput.Text, out int n))
                {
                    cancellationTokenSource = new CancellationTokenSource();
                    fibonacciWorker.RunWorkerAsync(n);
                    btnCalculate.Enabled = false;
                }
                else
                {
                    MessageBox.Show("invalid input");
                }
            }
        }
    }
}
