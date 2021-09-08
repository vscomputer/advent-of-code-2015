using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day05Tests
    {
        [Test]
        public void PairIsNice_NicePair_NotRejected()
        {
            var subject = new EvaluatesChild();
            bool result = subject.PairIsNice("aa");
            result.Should().Be(true);
        }

        [Test]
        public void PairIsNaughty_NaughtyPairs_Rejected()
        {
            //ab, cd, pq, or xy
            var subject = new EvaluatesChild();
            subject.PairIsNice("ab").Should().BeFalse();
            subject.PairIsNice("cd").Should().BeFalse();
            subject.PairIsNice("pq").Should().BeFalse();
            subject.PairIsNice("xy").Should().BeFalse();
        }

        [Test]
        public void LatePairIsNaughty_Rejected()
        {
            var subject = new EvaluatesChild();
            bool result = subject.AllPairsAreNice("aaab");
            result.Should().BeFalse();
        }

        [Test]
        public void LargeString_AllPairsAreNice_ReturnsTrue()
        {
            var subject = new EvaluatesChild();
            subject.AllPairsAreNice("ugknbfddgicrmopn").Should().BeTrue();
        }

        [Test]
        public void ContainsTwin_NoTwin_ReturnsFalse()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ContainsTwin("abcdef");
            result.Should().BeFalse();
        }

        [Test]
        public void ContainsTwin_Twin_ReturnsTrue()
        {
            var subject = new EvaluatesChild();
            subject.ContainsTwin("xx").Should().BeTrue();
            subject.ContainsTwin("abcdde").Should().BeTrue();
            subject.ContainsTwin("aabbccdd").Should().BeTrue();
        }

        [Test]
        public void ContainsThreeVowels_TwoVowels_ReturnsFalse()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ContainsThreeVowels("abcde");
            result.Should().BeFalse();
        }

        [Test]
        public void ContainsThreeVowels_ThreeVowels_ReturnsTrue()
        {
            var subject = new EvaluatesChild();
            subject.ContainsThreeVowels("aei").Should().BeTrue();
            subject.ContainsThreeVowels("xazegov").Should().BeTrue();
        }

        [Test]
        public void SampleInput_Nice_ReturnsNice()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ChildIsNice("ugknbfddgicrmopn");
            result.Should().BeTrue("because it has three vowels, a twin, and no disallowed pairs");
            subject.ChildIsNice("aaa").Should().BeTrue();
        }

        [Test]
        public void SampleInput_Naughty_ReturnsNaughty()
        {
            var subject = new EvaluatesChild();
            subject.ChildIsNice("jchzalrnumimnmhp").Should().BeFalse("no double letter");
            subject.ChildIsNice("haegwjzuvuyypxyu").Should().BeFalse("contains xy");
            subject.ChildIsNice("dvszwmarrgswjxmb").Should().BeFalse("only one vowel");
        }

        [Test]
        public void ChildIsNice_PuzzleInput_ReturnsTheAnswer()
        {
            var inputs = File.ReadAllLines(@"C:\Projects\Homework\advent-of-code-2015\Advent2015\input-day5.txt");
            var subject = new EvaluatesChild();
            int result = inputs.Count(input => subject.ChildIsNice(input));

            result.Should().Be(238);
        }
    }

    public class EvaluatesChild
    {
        public bool PairIsNice(string input)
        {
            return input != "ab" && input != "cd" && input != "pq" && input != "xy";
        }

        public bool ContainsTwin(string input)
        {
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == input[i - 1])
                {
                    return true;
                }
            }

            return false;

        }

        public bool ContainsThreeVowels(string input)
        {
            int numVowels = 0;
            foreach (var t in input)
            {
                if (t == 'a' || t == 'e' || t == 'i' || t == 'o' || t == 'u')
                {
                    numVowels++;
                }

                if (numVowels >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ChildIsNice(string input)
        {
            bool containsThreeVowels = ContainsThreeVowels(input);
            bool containsTwin = ContainsTwin(input);
            bool allPairsAreNice = AllPairsAreNice(input);

            return (containsThreeVowels && containsTwin && allPairsAreNice);
        }

        public bool AllPairsAreNice(string input)
        {
            for (int i = 1; i < input.Length; i++)
            {
                if (PairIsNice(input[i - 1].ToString() + input[i].ToString()) == false)
                    return false;
            }

            return true;
        }
    }
}