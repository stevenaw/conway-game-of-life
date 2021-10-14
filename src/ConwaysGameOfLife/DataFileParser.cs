using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public class DataFileParser
    {
        public async Task<bool[][]> ParseAsync(StreamReader reader)
        {
            var data = new List<bool[]>();

            while (!reader.EndOfStream)
                data.Add(Parse(await reader.ReadLineAsync()));

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
