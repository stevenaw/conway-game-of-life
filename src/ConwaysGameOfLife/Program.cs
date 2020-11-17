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
                var delay = TimeSpan.FromMilliseconds(opts.Delay);
                var game = await Initialize(opts);

                Console.Clear();
                Console.CursorVisible = false;

                await Console.Out.WriteAsync($"Press Ctrl+C  or any key to exit");

                await game.Run(
                    delay,
                    opts.GenerationCount,
                    Render,
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

        private static void Render(Life game)
        {
            const char DEAD_CELL = ' ';
            const char LIVE_CELL = 'X';

            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"Generation: {game.CurrentGeneration.N.ToString()}");

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
