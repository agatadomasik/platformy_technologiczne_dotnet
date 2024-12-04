using System;
using System.Threading.Tasks;

class Program
{
    delegate long FactorialDelegate(int n);

    static async Task Main(string[] args)
    {
        int N = 5; // Przykładowe wartości N i K
        int K = 2;

        await TaskExample(N, K);

        await DelegateExample(N, K);

        await AsyncAwaitExample(N, K);
    }

    static async Task TaskExample(int N, int K)
    {
        Console.WriteLine("Task and Task<T> implementation:");

        Task<long> numeratorTask = Task.Run(() => Factorial(N));
        Task<long> denominatorTask = Task.Run(() => Factorial(K) * Factorial(N - K));

        await Task.WhenAll(numeratorTask, denominatorTask);

        long result = numeratorTask.Result / denominatorTask.Result;
        Console.WriteLine($"Newton symbol for N={N} i K={K} is: {result}");
    }

    static async Task DelegateExample(int N, int K)
    {
        Console.WriteLine("\n delegate implementation:");

        Func<int, long> numeratorFunc = Factorial;
        Func<int, long> denominatorFunc = Factorial;

        Task<long> numeratorTask = Task.Run(() => numeratorFunc(N));
        Task<long> denominatorTask = Task.Run(() => denominatorFunc(K) * denominatorFunc(N - K));

        await Task.WhenAll(numeratorTask, denominatorTask);

        long result = numeratorTask.Result / denominatorTask.Result;
        Console.WriteLine($"Newton symbol dla N={N} i K={K} is: {result}");
    }

    static async Task AsyncAwaitExample(int N, int K)
    {
        Console.WriteLine("\nsync-await implementation:");

        long numerator = await Task.Run(() => Factorial(N));
        long denominator = await Task.Run(() => Factorial(K) * Factorial(N - K));

        long result = numerator / denominator;
        Console.WriteLine($"Newton symbol for N={N} i K={K} is: {result}");
    }

    static long Factorial(int n)
    {
        long result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
}
