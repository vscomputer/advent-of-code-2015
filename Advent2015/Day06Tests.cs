using System.Runtime.InteropServices;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day06Tests
    {
        [Test]
        public void NewGrid_Returns0()
        {
            var subject = new LightGrid();
            int result = subject.SumOfLitCells();
            result.Should().Be(0);
        }
    }

    public class LightGrid
    {
        private int[,] _cells = new int[999,999];
        public int SumOfLitCells()
        {
            return 0;
        }
    }
}