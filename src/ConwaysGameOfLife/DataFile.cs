using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public static class DataFile
    {
        public static async Task<bool[][]> ParseAsync(IAsyncEnumerable<string> lines)
        {
            var data = new List<bool[]>();

            await foreach (var line in lines)
                data.Add(Parse(line));

            if (data.Count > 0)
            {
                var firstLen = data[0].Length;
                for(var i = 1; i < data.Count; i++)
                {
                    if (data[i].Length != firstLen)
                        throw new InvalidOperationException("Input file contained varying lengths");
                }
            }

            return data.ToArray();
        }

        private static bool[] Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                return Array.Empty<bool>();
            else
            {
                var result = GC.AllocateUninitializedArray<bool>(line.Length);

                for (var i = 0; i < line.Length; i++)
                    result[i] = line[i] != '0';

                return result;
            }
        }
    }
}
