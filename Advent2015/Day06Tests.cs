using System;
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

        [Test]
        public void ParseCommand_TurnOn_GetsArgs()
        {
            var subject = new ParsesCommand();
            var result = subject.Parse("turn on 0,0 through 999,999");
            var expected = new GridCommand {Type = GridCommandType.TurnOn, X1 = 0, Y1 = 1, X2 = 999, Y2 = 999};
            Assert.AreEqual(result.Type, GridCommandType.TurnOn);
            result.X1.Should().Be(0);
            result.Y1.Should().Be(0);
            result.X2.Should().Be(999);
            result.Y2.Should().Be(999);
        }
    }

    public enum GridCommandType
    {
        TurnOn,
        Toggle,
        TurnOff
    }
    
    public class GridCommand
    {
        public GridCommandType Type { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }

    public class ParsesCommand
    {
        public GridCommand Parse(string input)
        {
            //turn on 0,0 through 999,999
            var result = new GridCommand();
            int typeLength = 0;
            if (input.StartsWith("turn on"))
            {
                result.Type = GridCommandType.TurnOn;
                typeLength = "turn on".Length;
            }

            var splitter = new string[] {"through"};
            var tokens = input.Substring(typeLength).Split(splitter, StringSplitOptions.None);
            var pos1 = tokens[0].Split(',');
            result.X1 = int.Parse(pos1[0]);
            result.Y1 = int.Parse(pos1[1]);
            var pos2 = tokens[1].Split(',');
            result.X2 = int.Parse(pos2[0]);
            result.Y2 = int.Parse(pos2[1]);
            
            return result;
        }
    }

    public class LightGrid
    {
        private int[,] _cells = new int[1000,1000];
        public int SumOfLitCells()
        {
            return 0;
        }
    }
}