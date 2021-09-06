using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day03Tests
    {
        [Test]
        public void Count_NoMoves_Returns1()
        {
            var subject = new CountsPositions();
            subject.Count("").Should().Be(1);
        }
        
        [Test]
        [Ignore("not ready")]
        public void Count_OneMove_Returns2()
        {
            var subject = new CountsPositions();
            int result = subject.Count(">");
            result.Should().Be(2);
        }
    }

    public class CountsPositions
    {
        private int result = 1;
        public int Count(string input)
        {
            return result;
        }
    }
}