using BenchmarkDotNet.Running;

namespace LargeFileGenerator.Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkDemo>();
        }
    }
}
