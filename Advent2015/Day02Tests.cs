using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day02Tests
    {
        [Test]
        public void GetDimensions_FirstSampleInput_GetsDimension()
        {
            var expected = new List<int> {2, 3, 4};
            var subject = new GetsDimensions();
            subject.GetDimensions("2x3x4").Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPaperDimensionsWithoutSlack_FirstSampleInput_GetsCalculatedPaper()
        {
            var getsDimensions = new GetsDimensions();
            var dimensions = getsDimensions.GetDimensions("2x3x4");

            var subject = new CalculatesWrappingPaper();
            int result = subject.CalculateWithoutSlack(dimensions);
            result.Should().Be(52);
        }

        [Test]
        public void GetPaperDimensionsWithSlack_FirstSampleInput_GetsCalculatedPaper()
        {
            var getsDimensions = new GetsDimensions();
            var dimensions = getsDimensions.GetDimensions("2x3x4");

            var subject = new CalculatesWrappingPaper();
            int result = subject.CalculateWithSlack(dimensions);
            result.Should().Be(58);
        }

        [Test]
        public void GetPaperDimensionsWithSlack_2ndSampleInput_GetsCalculatedPaper()
        {
            var getsDimensions = new GetsDimensions();
            var dimensions = getsDimensions.GetDimensions("1x1x10");

            var subject = new CalculatesWrappingPaper();
            int result = subject.CalculateWithSlack(dimensions);
            result.Should().Be(43);
        }

        [Test]
        public void GetPaperDimensionsWithSlack_CombinedFromRealInput_GetsTheAnswer()
        {
            var lines = File.ReadAllLines(@"C:\Projects\Homework\advent-of-code-2015\Advent2015\input-day2.txt");
            var result = 0;
            foreach (var line in lines)
            {
                var getsDimensions = new GetsDimensions();
                var dimensions = getsDimensions.GetDimensions(line);

                var subject = new CalculatesWrappingPaper();
                result += subject.CalculateWithSlack(dimensions);
            }

            result.Should().Be(1588178);
        }
    }

    public class CalculatesWrappingPaper
    {
        private int _minSideArea = int.MaxValue;
        public int CalculateWithoutSlack(IEnumerable<int> dimensions)
        {
            var result = 0;
            var sides = dimensions.ToList();
            result += (2 * GetSide(sides[0], sides[1]));
            result += (2 * GetSide(sides[1], sides[2]));
            result += (2 * GetSide(sides[2], sides[0]));
            return result;
        }

        private int GetSide(int x , int y)
        {
            var result = x * y;
            if (_minSideArea > result)
            {
                _minSideArea = result;
            }

            return result;
        }

        public int CalculateWithSlack(IEnumerable<int> dimensions)
        {
            var paperWithoutSlack = CalculateWithoutSlack(dimensions);
            return _minSideArea + paperWithoutSlack;

        }
    }

    public class GetsDimensions
    {
        public IEnumerable<int> GetDimensions(string inputString)
        {
            return inputString.Split('x').Select(int.Parse).ToList();
        }
    }
}