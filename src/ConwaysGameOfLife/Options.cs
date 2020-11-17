using CommandLine;
using System;

namespace ConwaysGameOfLife
{
    public class Options
    {
        [Option('l', "length", Required = false, Default = 500)]
        public int GenerationLength { get; set; }

        [Option('c', "count", Required = false)]
        public int GenerationCount { get; set; } = Int32.MaxValue;

        [Option('f', "file", Required = true)]
        public string InputFile { get; set; }
    }
}
