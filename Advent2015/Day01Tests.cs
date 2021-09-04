using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day01Tests
    {
        
        [Test]
        public void InitIs0()
        {
            var subject = new FindsFloorNumber();
            subject.Find("").Should().Be(0);
        }

        [Test]
        public void Find_OpenParen_Adds1()
        {
            var subject = new FindsFloorNumber();
            subject.Find("(").Should().Be(1);
        }

        [Test]
        public void Find_CloseParen_Subtracts1()
        {
            var subject = new FindsFloorNumber();
            subject.Find(")").Should().Be(-1);
        }

        [Test]
        public void Find_Multiple_ResultsIn3()
        {
            var subject = new FindsFloorNumber();
            subject.Find("(((").Should().Be(3);
        }

        [Test]
        public void Find_MultipleMixed_ResultsIn3()
        {
            var subject = new FindsFloorNumber();
            subject.Find("(()(()(").Should().Be(3);
        }

        [Test]
        public void Find_Multiple_ResultsInMinus1()
        {
            var subject = new FindsFloorNumber();
            subject.Find("())").Should().Be(-1);
        }

        [Test]
        public void Find_RealInput1_ResultsInAnswer()
        {
            var subject = new FindsFloorNumber();
            var input = File.ReadAllText(@"C:\Projects\Homework\advent-of-code-2015\Advent2015\input-day1.txt");
            subject.Find(input).Should().Be(74);
        }

        [Test]
        public void GetBasementPosition_OneParen_1()
        {
            var subject = new FindsFloorNumber();
            subject.Find(")");
            subject.GetBasementStep().Should().Be(1);
        }

        [Test]
        public void GetBasementStep_MultipleParens_5()
        {
            var subject = new FindsFloorNumber();
            subject.Find("()())");
            subject.GetBasementStep().Should().Be(5);
        }

        [Test]
        public void GetBasementStep_RealInput()
        {
            var subject = new FindsFloorNumber();
            var input = File.ReadAllText(@"C:\Projects\Homework\advent-of-code-2015\Advent2015\input-day1.txt");
            subject.Find(input);
            subject.GetBasementStep().Should().Be(1795);
        }

    }

    public class FindsFloorNumber
    {
        private int _currentFloor ;
        private int _steps;
        private bool _basementFound = false;
        public int Find(string parens)
        {
            foreach (char paren in parens)
            {
                if (_currentFloor > -1 && _basementFound == false)
                {
                    _steps++;
                }

                if (_currentFloor == -1)
                {
                    _basementFound = true;
                }
                
                if (paren == '(')
                {
                    _currentFloor++;
                }

                if (paren == ')')
                {
                    _currentFloor--;
                }
            }
            
            return _currentFloor;
        }

        public int GetBasementStep()
        {
            return _steps;
        }
    }
}