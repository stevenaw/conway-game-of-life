using CommandLine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async opts =>
            {
                var generationLength = TimeSpan.FromMilliseconds(opts.GenerationLength);
                var game = await Initialize(opts);

                var generationCount = opts.GenerationCount <= 0 ? int.MaxValue : opts.GenerationCount;

                Console.Clear();
                Console.CursorVisible = false;

                await Console.Out.WriteAsync($"Press Ctrl+C  or any key to exit");

                await game.Run(
                    generationLength,
                    generationCount,
                    game => Render(game, generationCount),
                    () => Console.KeyAvailable);
            });
        }

        private static async Task<Life> Initialize(Options opts)
        {
            var parser = new DataFileParser();

            using var f = File.OpenRead(opts.InputFile);
            using var reader = new StreamReader(f);

            var data = await parser.ParseAsync(reader);

            return new Life(data);
        }

        private static void Render(Life game, int generationCount)
        {
            const char DEAD_CELL = ' ';
            const char LIVE_CELL = 'X';

            Console.SetCursorPosition(0, 1);

            if (generationCount == int.MaxValue)
                Console.WriteLine($"Generation: {game.CurrentGeneration.N.ToString()}");
            else
            {
                var genCountStr = generationCount.ToString();
                Console.WriteLine($"Generation: {game.CurrentGeneration.N.ToString().PadLeft(genCountStr.Length)} / {genCountStr}");
            }

            Console.Write('/');
            for (var i = 0; i < game.Width; i++)
                Console.Write('-');
            Console.WriteLine('\\');

            foreach (var row in game.CurrentGeneration.Data)
            {
                Console.Write('|');

                foreach (var col in row)
                    Console.Write(col ? LIVE_CELL : DEAD_CELL);

                Console.WriteLine('|');
            }

            Console.Write('\\');
            for (var i = 0; i < game.Width; i++)
                Console.Write('-');
            Console.WriteLine('/');
        }
    }
}
