using CommandLine;

namespace ConwaysGameOfLife
{
    public class Options
    {
        [Option('d', "delay", Required = false, Default = 500)]
        public int Delay { get; set; }

        [Option('n', "numGenerations", Required = false, Default = null)]
        public int? GenerationCount { get; set; }

        [Option('f', "file", Required = true)]
        public string InputFile { get; set; }
    }
}
