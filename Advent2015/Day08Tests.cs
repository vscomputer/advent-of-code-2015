using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace Advent2015
{
    [TestFixture]
    public class Day08Tests
    {
        [Test]
        public void GetLengths_DoubleQuote_Gets2And0()
        {
            var lines = File.ReadAllLines(
                "C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\day-8-sample-input-1.txt");

            var subject = new GetsLengths();
            Lengths result = subject.Get(lines[0]);

            result.InString.Should().Be(2);
            result.InMemory.Should().Be(0);
        }
    }

    public struct Lengths
    {
        public int InString;
        public int InMemory;
    }

    public class GetsLengths
    {
        public Lengths Get(string line)
        {
            var result = 
                new Lengths {
                    InString = line.Length,
                    InMemory = CalculateInMemoryCharacters(line)};
            return result;
        }

        private int CalculateInMemoryCharacters(string line)
        {
            var origArray = line.ToCharArray();
            List<char> result = new List<char>();
            foreach (var c in origArray)
            {
                if (c != '"')
                {
                    result.Add(c);
                }
            }

            return result.Count;
        }
    }
}