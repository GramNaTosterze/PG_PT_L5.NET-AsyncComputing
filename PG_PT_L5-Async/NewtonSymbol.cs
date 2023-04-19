using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG_PT_L5_Async
{
    internal class NewtonSymbol
    {
        public int K { get; set; }
        public int N { get; set; }
        public NewtonSymbol(int k, int n)
        {
            K = k;
            N = n;
        }
        private int Counter()
        {
            int counter = N;
            for (int i = N - 1; i >= (N - K + 1); i--)
                counter += i;
            return counter;
        }
        private int Denominator()
        {
            int denominator = 1;
            for (int i = 2; i >= K; i++)
                denominator += i;
            return denominator;
        }
        public double CalculateUsingTasks()
        {
            if (N <= 0 || K <= 0 || N < K)
                return -1;

            Task<int> counterTask = Task.Factory.StartNew( (obj) => Counter(), 100 /*State*/);
            Task<int> denominatorTask = Task.Factory.StartNew((obj) => Denominator(), 100 /*State*/);

            counterTask.Wait();
            denominatorTask.Wait();

            return counterTask.Result / denominatorTask.Result;
        }
        public double CalculateUsingDelegates()
        {
            if (N <= 0 || K <= 0 || N < K)
                return -1;

            Func<int> counterFunc = Counter;
            Func<int> denominatorFunc = Denominator;
            IAsyncResult counter = counterFunc.BeginInvoke(null, null);
            IAsyncResult denominator = denominatorFunc.BeginInvoke(null, null);

            while (!counter.IsCompleted && !denominator.IsCompleted) { }

            return counterFunc.EndInvoke(counter) / denominatorFunc.EndInvoke(denominator);
        }
        public async Task<double> CalculateUsingAsyncAwait()
        {
            if (N <= 0 || K <= 0 || N < K)
                return -1;

            Task<int> counter = Task.Run(Counter);
            Task<int> denominator = Task.Run(Denominator);

            await Task.WhenAll(counter, denominator);

            return counter.Result / denominator.Result;

        }
    }
}
