using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    public class DataFileParser
    {
        private List<bool[]> Data { get; } = new List<bool[]>();

        private void InitializeParser()
        {
            Data.Clear();
        }

        public async Task<bool[][]> ParseAsync(StreamReader reader)
        {
            InitializeParser();

            while (!reader.EndOfStream)
                Parse(await reader.ReadLineAsync());

            if (Data.Count > 0)
            {
                var firstLen = Data[0].Length;

                for(var i = 1; i < Data.Count; i++)
                {
                    if (Data[i].Length != firstLen)
                        throw new InvalidOperationException("Input file contained varying lengths");
                }
            }

            return Data.ToArray();
        }

        private void Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                Data.Add(Array.Empty<bool>());
            else
            {
                var result = GC.AllocateUninitializedArray<bool>(line.Length);

                for (var i = 0; i < line.Length; i++)
                    result[i] = line[i] != '0';

                Data.Add(result);
            }
        }
    }
}
