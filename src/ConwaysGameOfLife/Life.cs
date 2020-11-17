using System;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public readonly struct LifeGeneration
    {
        public readonly int N { get; init; }
        public readonly bool[][] Data { get; init; }
    }

    public class Life
    {
        public LifeGeneration CurrentGeneration { get; private set; }

        public int Width { get; }
        public int Height { get; }

        public Life(bool[][] data)
        {
            Height = data.Length;
            Width = data.Length > 0 ? data[0].Length : 0;

            CurrentGeneration = new LifeGeneration()
            {
                N = 0,
                Data = data
            };
        }

        public async Task Run(
            TimeSpan generationLength,
            int? maxGenerationCount,
            Action<Life> render,
            Func<bool> exit)
        {
            var maxGenerations = Math.Clamp(maxGenerationCount ?? int.MaxValue, 1, int.MaxValue);

            while (CurrentGeneration.N <= maxGenerations)
            {
                render(this);
                await Task.Delay(generationLength);

                if (exit())
                    break;

                Evolve();
            }
        }

        public void Evolve()
        {
            var currentGen = CurrentGeneration.Data;
            bool[][] nextGen = GC.AllocateUninitializedArray<bool[]>(currentGen.Length);

            for (var x = 0; x < nextGen.Length; x++)
            {
                nextGen[x] = GC.AllocateUninitializedArray<bool>(currentGen[x].Length);

                for (var y = 0; y < nextGen[x].Length; y++)
                {
                    var liveNeighbours = CountLiveNeighbours(currentGen, x, y);
                    if (currentGen[x][y])
                        nextGen[x][y] = (liveNeighbours == 2 || liveNeighbours == 3);
                    else
                        nextGen[x][y] = (liveNeighbours == 3);
                }
            }

            CurrentGeneration = new LifeGeneration()
            {
                N = CurrentGeneration.N + 1,
                Data = nextGen
            };
        }

        private static int CountLiveNeighbours(bool[][] data, int x, int y)
        {
            var count = 0;

            for (var xCount = Math.Max(x - 1, 0); xCount <= Math.Min(x + 1, data.Length - 1); xCount++)
            {
                for (var yCount = Math.Max(y - 1, 0); yCount <= Math.Min(y + 1, data[xCount].Length - 1); yCount++)
                {
                    if (data[xCount][yCount] && (xCount != x || yCount != y))
                        count++;
                }
            }

            return count;
        }
    }
}
