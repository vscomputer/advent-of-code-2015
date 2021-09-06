using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public void Count_OneMove_Returns2()
        {
            var subject = new CountsPositions();
            int result = subject.Count(">");
            result.Should().Be(2);
        }

        [Test]
        public void Count_FourMovesBackToHome_Returns4()
        {
            var subject = new CountsPositions();
            int result = subject.Count("^>v<");
            result.Should().Be(4);
        }

        [Test]
        public void Count_MovesBetweenTwoHouses_Returns2()
        {
            var subject = new CountsPositions();
            int result = subject.Count("^v^v^v^v^v");
            result.Should().Be(2);
        }

        [Test]
        public void Count_UsesPuzzleInput_GetsAnswer1()
        {
            var input = File.ReadAllText("C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\input-day3.txt");
            var subject = new CountsPositions();
            int result = subject.Count(input);
            result.Should().Be(2081);
        }

        [Test]
        public void CountWithRobot_FirstSampleInput_Returns3()
        {
            var subject = new CountsPositions();
            int result = subject.CountWithRobot("^v");
            result.Should().Be(3);
        }

        [Test]
        public void CountWithRobot_SecondSampleInput_Returns3()
        {
            var subject = new CountsPositions();
            int result = subject.CountWithRobot("^>v<");
            result.Should().Be(3);
        }

        [Test]
        public void CountWithRobot_ThirdSampleInput_Returns11()
        {
            var subject = new CountsPositions();
            int result = subject.CountWithRobot("^v^v^v^v^v");
            result.Should().Be(11);
        }

        [Test]
        public void CountWithRobot_PuzzleInput_ReturnsTheAnswer()
        {
            var input = File.ReadAllText("C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\input-day3.txt");
            var subject = new CountsPositions();
            int result = subject.CountWithRobot(input);
            result.Should().Be(2341);
        }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class CountsPositions
    {
        private readonly List<Position> _positions;
        private Position _santaPosition;
        private Position _robotSantaPosition;
        
        public CountsPositions()
        {
            Position home = new Position(0, 0);
            _positions = new List<Position> {home};
            _santaPosition = home;
            _robotSantaPosition = home;
        }

        public int Count(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return _positions.Distinct().Count();
            
            foreach (var move in input)
            {
                var nextPosition = new Position(_santaPosition.X, _santaPosition.Y);
                    
                switch (move)
                {
                    case '>':
                        nextPosition.X++;
                        break;
                    case '<':
                        nextPosition.X--;
                        break;
                    case '^':
                        nextPosition.Y++;
                        break;
                    case 'v':
                        nextPosition.Y--;
                        break;
                    default:
                        throw new ArgumentException("not a valid move");
                }

                if (_positions.Exists(n => n.X == nextPosition.X && n.Y == nextPosition.Y) == false)
                {
                    _positions.Add(nextPosition);
                }

                _santaPosition = nextPosition;
            }
            return _positions.Count();
        }

        public int CountWithRobot(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return _positions.Distinct().Count();
            if (string.IsNullOrWhiteSpace(input)) return _positions.Distinct().Count();
            int steps = input.Length;

            for (int i = 0; i < steps; i++)
            {
                Position nextPosition;
                if (i % 2 == 0)
                {
                    nextPosition = new Position(_santaPosition.X, _santaPosition.Y);
                }
                else
                {
                    nextPosition = new Position(_robotSantaPosition.X, _robotSantaPosition.Y);
                }

                switch (input[i])
                {
                    case '>':
                        nextPosition.X++;
                        break;
                    case '<':
                        nextPosition.X--;
                        break;
                    case '^':
                        nextPosition.Y++;
                        break;
                    case 'v':
                        nextPosition.Y--;
                        break;
                    default:
                        throw new ArgumentException("not a valid move");
                }

                if (_positions.Exists(n => n.X == nextPosition.X && n.Y == nextPosition.Y) == false)
                {
                    _positions.Add(nextPosition);
                }

                if (i % 2 == 0)
                {
                    _santaPosition = nextPosition;
                }
                else
                {
                    _robotSantaPosition = nextPosition;
                }
            }

            return _positions.Count();
        }
    }
}