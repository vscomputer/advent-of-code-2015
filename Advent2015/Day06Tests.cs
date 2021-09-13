using System;
using System.IO;
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
            var result = subject.Parse("turn on 0,1 through 999,999");
            Assert.AreEqual(result.Type, GridCommandType.TurnOn);
            result.X1.Should().Be(0);
            result.Y1.Should().Be(1);
            result.X2.Should().Be(999);
            result.Y2.Should().Be(999);
        }

        [Test]
        public void PassCommand_1Cell_Returns1()
        {
            var parser = new ParsesCommand();
            var command = parser.Parse("turn on 42, 42 through 42,42");

            var subject = new LightGrid();
            subject.ProcessCommand(command);
            subject.SumOfLitCells().Should().Be(1);

        }

        [Test]
        public void PassCommand_TurnOnEveryLight_ReturnsOneMillion()
        {
            var parser = new ParsesCommand();
            var command = parser.Parse("turn on 0, 0 through 999,999");

            var subject = new LightGrid();
            subject.ProcessCommand(command);
            subject.SumOfLitCells().Should().Be(1000000);
        }

        [Test]
        [Ignore("slow")]
        public void PuzzleInput_ReturnsAnswer()
        {
            var lines = File.ReadAllLines("C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\input-day6.txt");
            var parser = new ParsesCommand();
            var subject = new LightGrid();

            foreach (var line in lines)
            {
                var command = parser.Parse(line);
                subject.ProcessCommand(command);
            }

            subject.SumOfLitCells().Should().Be(-1);
        }

        [Test]
        [Ignore("not needed")]
        public void testingStuff()
        {
            var lines = File.ReadAllLines("C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\day-6-simplified-1.txt");
            var parser = new ParsesCommand();
            var subject = new LightGrid();

            foreach (var line in lines)
            {
                var command = parser.Parse(line);
                subject.ProcessCommand(command);
                int tempResult = subject.SumOfLitCells();
            }

            subject.SumOfLitCells().Should().Be(-1);
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
            var result = new GridCommand();
            int typeLength ;
            if (input.StartsWith("turn on"))
            {
                result.Type = GridCommandType.TurnOn;
                typeLength = "turn on".Length;
            }
            else if (input.StartsWith("toggle"))
            {
                result.Type = GridCommandType.Toggle;
                typeLength = "toggle".Length;
            }
            else if (input.StartsWith("turn off"))
            {
                result.Type = GridCommandType.TurnOff;
                typeLength = "turn off".Length;
            }
            else
            {
                throw new ArgumentException("unfamiliar command type");
            }

            var splitter = new[] {"through"};
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
            int result = 0;
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    result += _cells[i, j];
                }
            }

            return result;
        }

        public void ProcessCommand(GridCommand command)
        {
            for (int i = command.X1; i <= command.X2; i++)
            {
                for (int j = command.Y1; j <= command.Y2; j++)
                {
                    if (command.Type == GridCommandType.TurnOn)
                    {
                        _cells[i, j] += 1;
                    }

                    if (command.Type == GridCommandType.TurnOff)
                    {
                        _cells[i, j] -= 1;
                        if (_cells[i, j] < 0)
                        {
                            _cells[i, j] = 0;
                        }
                    }

                    if (command.Type == GridCommandType.Toggle)
                    {
                        // if (_cells[i, j] == 0)
                        // {
                        //     _cells[i, j] = 1;
                        // }
                        // else if (_cells[i, j] == 1)
                        // {
                        //     _cells[i, j] = 0;
                        // }
                        _cells[i, j] += 2;
                    }
                }
            }
        }
    }
}