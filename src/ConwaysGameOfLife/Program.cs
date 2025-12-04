using CommandLine;
using System.IO;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConwaysGameOfLife
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async opts =>
            {
                var generationLength = TimeSpan.FromMilliseconds(opts.GenerationLength);
                var gameData = await DataFile.ParseAsync(File.ReadLinesAsync(opts.InputFile));
                var game = new Life(gameData);

                var generationCount = opts.GenerationCount <= 0 ? int.MaxValue : opts.GenerationCount;

                Console.Clear();
                Console.CursorVisible = false;

                await Console.Out.WriteAsync("Press Ctrl+C to exit");

                using var cts = new CancellationTokenSource();

                Console.CancelKeyPress += (sender, e) =>
                {
                    cts.Cancel();
                    e.Cancel = true; // Allow us to gracefully end it
                };

                await game.RunAsync(
                    generationLength,
                    generationCount,
                    game => Render(game, generationCount),
                    cts.Token);
            });
        }

        private static void Render(Life game, int generationCount)
        {
            const char DEAD_CELL = ' ';
            const char LIVE_CELL = 'X';

            Console.SetCursorPosition(0, 1);

            if (generationCount == int.MaxValue)
                Console.WriteLine($"Generation: {game.CurrentGeneration.N}");
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
