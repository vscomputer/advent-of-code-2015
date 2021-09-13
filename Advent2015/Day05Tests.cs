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
        [Ignore("Invalid on Day 2")]
        public void SampleInput_Nice_ReturnsNice()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ChildIsNice("ugknbfddgicrmopn");
            result.Should().BeTrue("because it has three vowels, a twin, and no disallowed pairs");
            subject.ChildIsNice("aaa").Should().BeTrue();
        }

        [Test]
        [Ignore("Invalid on Day 2")]
        public void SampleInput_Naughty_ReturnsNaughty()
        {
            var subject = new EvaluatesChild();
            subject.ChildIsNice("jchzalrnumimnmhp").Should().BeFalse("no double letter");
            subject.ChildIsNice("haegwjzuvuyypxyu").Should().BeFalse("contains xy");
            subject.ChildIsNice("dvszwmarrgswjxmb").Should().BeFalse("only one vowel");
        }

        [Test]
        //[Ignore("Takes a while")]
        public void ChildIsNice_PuzzleInput_ReturnsTheAnswer()
        {
            var inputs = File.ReadAllLines(@"C:\Projects\Homework\advent-of-code-2015\Advent2015\input-day5.txt");
            var subject = new EvaluatesChild();
            int result = inputs.Count(input => subject.ChildIsNice(input));

            result.Should().Be(69);
        }

        [Test]
        public void ContainsEchoChar_Doesnt_ReturnsFalse()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ContainsEcho("aabbccddee");
            result.Should().BeFalse();

            subject.ContainsEcho("abcdefghi").Should().BeFalse();
        }

        [Test]
        public void ContainsEcho_Does_ReturnsTrue()
        {
            var subject = new EvaluatesChild();
            subject.ContainsEcho("xyx").Should().BeTrue();
            subject.ContainsEcho("abcdefeghi").Should().BeTrue();
            subject.ContainsEcho("aaa").Should().BeTrue();
        }

        [Test]
        public void ContainsRepeatedPair_Doesnt_ReturnsFalse()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ContainsRepeatedPair("aad");
            result.Should().BeFalse();

            subject.ContainsRepeatedPair("ieodomkazucvgmuy").Should().BeFalse();
        }

        [Test]
        public void ContainsRepeatedPair_Does_ReturnsTrue()
        {
            var subject = new EvaluatesChild();
            bool result = subject.ContainsRepeatedPair("xyxy");
            result.Should().BeTrue();
            subject.ContainsRepeatedPair("aabcdefgaa").Should().BeTrue();
        }

        [Test]
        public void Day2_SampleInput_ReturnsTrue()
        {
            var subject = new EvaluatesChild();
            subject.ChildIsNice("qjhvhtzxzqqjkmpb").Should().BeTrue();
            subject.ChildIsNice("xxyxx").Should().BeTrue();
        }

        [Test]
        public void Day2_SampleInput_ReturnsFalse()
        {
            var subject = new EvaluatesChild();
            subject.ChildIsNice("uurcxstgmygtbstg").Should().BeFalse();
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
            bool containsEcho = ContainsEcho(input);
            bool containsRepeatedPair = ContainsRepeatedPair(input);

            return containsEcho && containsRepeatedPair;
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

        public bool ContainsEcho(string input)
        {
            for (int i = 2; i < input.Length; i++)
            {
                if (input[i - 2] == input[i])
                    return true;
            }

            return false;
        }

        public bool ContainsRepeatedPair(string input)
        {
            for (int i = 1; i < input.Length; i++)
            {
                var pair = input[i - 1].ToString() + input[i].ToString();
                if (input.Length - i < 2)
                    return false;
                for (int j = i + 2; j < input.Length; j++)
                {
                    if (pair == input[j - 1].ToString() + input[j])
                        return true;
                }
            }
            return false;
        }
    }
}