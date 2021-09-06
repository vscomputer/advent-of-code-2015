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
            subject.Get("2x3x4").Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPaperDimensionsWithoutSlack_FirstSampleInput_GetsCalculatedPaper()
        {
            var getsDimensions = new GetsDimensions();
            var dimensions = getsDimensions.Get("2x3x4");

            var subject = new CalculatesWrappingPaper();
            int result = subject.CalculateWithoutSlack(dimensions);
            result.Should().Be(52);
        }

        [Test]
        public void GetPaperDimensionsWithSlack_FirstSampleInput_GetsCalculatedPaper()
        {
            var getsDimensions = new GetsDimensions();
            var dimensions = getsDimensions.Get("2x3x4");

            var subject = new CalculatesWrappingPaper();
            int result = subject.CalculateWithSlack(dimensions);
            result.Should().Be(58);
        }

        [Test]
        public void GetPaperDimensionsWithSlack_2ndSampleInput_GetsCalculatedPaper()
        {
            var getsDimensions = new GetsDimensions();
            var dimensions = getsDimensions.Get("1x1x10");

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
                var dimensions = getsDimensions.Get(line);

                var subject = new CalculatesWrappingPaper();
                result += subject.CalculateWithSlack(dimensions);
            }

            result.Should().Be(1588178);
        }

        [Test]
        public void GetRibbonWithoutBow_FirstSampleInput_GetsExpectedResult()
        {
            var getsDimensions = new GetsDimensions();
            var sides = getsDimensions.Get("2x3x4").ToList();

            var subject = new GetsRibbonDimensions();
            var result = subject.Get(sides);

            result.Should().Be(10);
        }

        [Test]
        public void GetBowLength_FirstSampleInput_GetsExpectedResult()
        {
            var getsDimensions = new GetsDimensions();
            var sides = getsDimensions.Get("2x3x4").ToList();
            
            var subject = new GetsRibbonDimensions();
            var result = subject.GetBowLength(sides);
            result.Should().Be(24);
        }

        [Test]
        public void GetTotalRibbon_FirstSampleInput_GetsExpectedResult()
        {
            var getsDimensions = new GetsDimensions();
            var sides = getsDimensions.Get("2x3x4").ToList();
            var subject = new GetsRibbonDimensions();
            var result = subject.GetTotalRibbon(sides);
            result.Should().Be(34);
        }

        [Test]
        public void GetTotalRibbon_SecondSampleInput_GetsExpectedResult()
        {
            var getsDimensions = new GetsDimensions();
            var sides = getsDimensions.Get("1x1x10").ToList();
            var subject = new GetsRibbonDimensions();
            var result = subject.GetTotalRibbon(sides);
            result.Should().Be(14);
        }

        [Test]
        public void GetTotalRibbon_PuzzleInput_GetsTheAnswer()
        {
            var lines = File.ReadAllLines(@"C:\Projects\Homework\advent-of-code-2015\Advent2015\input-day2.txt");
            var result = 
                (from line in lines 
                    let getsDimensions = new GetsDimensions() 
                    select getsDimensions.Get(line).ToList() into sides 
                    let subject = new GetsRibbonDimensions() 
                    select subject.GetTotalRibbon(sides)).Sum();

            result.Should().Be(3783758);
        }
    }

    public class GetsRibbonDimensions
    {
        public int Get(List<int> sides)
        {
            var newSides = new List<int>(sides); //avoid mutating the input
            newSides.Remove(sides.Max());
            return newSides.Sum(side => 2 * side);
        }

        public int GetBowLength(List<int> sides)
        {
            var result = 1;
            foreach (var side in sides)
            {
                result *= side;
            }

            return result;
        }

        public int GetTotalRibbon(List<int> sides)
        {
            return GetBowLength(sides) + Get(sides);
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

        public IEnumerable<int> GetSides(IEnumerable<int> dimensions)
        {
            var result = new List<int>();
            var edges = dimensions.ToList();
            result.Add(edges[0] * edges[1]);
            result.Add(edges[1] * edges[2]);
            result.Add(edges[2] * edges[0]);
            return result;
        }

        public int GetRibbonAmountWithoutBow(IEnumerable<int> dimensions)
        {
            throw new System.NotImplementedException();
        }
    }

    public class GetsDimensions
    {
        public IEnumerable<int> Get(string inputString)
        {
            return inputString.Split('x').Select(int.Parse).ToList();
        }
    }
}