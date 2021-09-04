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
            subject.Find(input).Should().Be(42);
        }


    }

    public class FindsFloorNumber
    {
        private int _currentFloor ;
        public int Find(string parens)
        {
            foreach (char paren in parens)
            {
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
    }
}