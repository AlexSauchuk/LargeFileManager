using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace LargeFileGenerator.Benchmark
{
    [MemoryDiagnoser, SimpleJob(RunStrategy.Monitoring, iterationCount: 5)]
    public class BenchmarkDemo
    {
        private FileGenerator _fileGenerator;
        private IList<FileInfo> _generatedArtifacts;

        [GlobalSetup]
        public void Setup()
        {
            _fileGenerator = new FileGenerator();
            _generatedArtifacts = new List<FileInfo>();
        }

        [Benchmark]
        public void Generate1GBFile()
        {
            var artifact = _fileGenerator.Generate(2048);
            _generatedArtifacts.Add(artifact);
        }

        // [Benchmark]
        public void Generate5GBFile()
        {
            var artifact = _fileGenerator.Generate(5120);
            _generatedArtifacts.Add(artifact);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            foreach (var artifact in _generatedArtifacts)
            {
               // artifact.Delete();
            }
        }
    }
}
