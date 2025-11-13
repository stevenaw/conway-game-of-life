using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConwaysGameOfLife.Tests
{
    public class DataFileTests
    {

        [Test]
        public async Task ParseAsync_EmptySequence_ReturnsEmptyArray()
        {
            var result = await DataFile.ParseAsync(MakeAsync());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.Zero);
        }

        [Test]
        public async Task ParseAsync_ParsesLinesCorrectly()
        {
            var result = await DataFile.ParseAsync(MakeAsync("010", "111"));

            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result[0], Has.Length.EqualTo(3));
            Assert.That(result[1], Has.Length.EqualTo(3));

            Assert.That(result[0], Is.EqualTo([false, true, false]));
            Assert.That(result[1], Is.EqualTo([true, true, true]));
        }

        [Test]
        public async Task ParseAsync_EmptyLine_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await DataFile.ParseAsync(MakeAsync(string.Empty));
            });
        }

        [Test]
        public void ParseAsync_VaryingLengths_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await DataFile.ParseAsync(MakeAsync("0", "01"));
            });
        }

        [Test]
        public async Task ParseAsync_SingleLine_ParsesCorrectly()
        {
            var result = await DataFile.ParseAsync(MakeAsync("1010"));

            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0], Is.EqualTo([true, false, true, false]));
        }

        private static async IAsyncEnumerable<string> MakeAsync(params string[] items)
        {
            foreach (var s in items)
            {
                await Task.Yield();
                yield return s;
            }
        }
    }
}
