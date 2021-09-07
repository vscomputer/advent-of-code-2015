using System;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day04Tests
    {
        //Trying algorithm stated here: https://www.c-sharpcorner.com/blogs/creation-of-md5-encryption-in-c-sharp

        [Test]
        public void GetHash_KnownString_ReturnsKnownHash()
        {
            var md5Hasher = new Md5Hasher();
            string result = md5Hasher.Hash("Hello World!");
            result.Should().Be("ED076287532E86365E841E92BFC50D8C");
        }

        [Test]
        [Ignore("takes a long time")]
        public void Mine_FirstExample_ReturnsExpectedInt()
        {
            var mines = new MinesAdventCoins();
            int result = mines.Mine("abcdef");
            result.Should().Be(609043);
        }
        
        [Test]
        [Ignore("takes a long time")]
        public void Mine_SecondExample_ReturnsExpectedInt()
        {
            var mines = new MinesAdventCoins();
            int result = mines.Mine("pqrstuv");
            result.Should().Be(1048970);
        }
        
        [Test]
        [Ignore("takes a long time")]
        public void Mine_PuzzleInput_ReturnsAnswer()
        {
            var mines = new MinesAdventCoins();
            int result = mines.Mine("yzbqklnj");
            result.Should().Be(9962624);
        }
        
    }

    public class MinesAdventCoins
    {
        public int Mine(string secret)
        {
            var md5Hasher = new Md5Hasher();
            for (int i = 0; i < 1000000000; i++)
            {
                if (md5Hasher.Hash(secret + i.ToString()).StartsWith("000000"))
                    return i;
            }

            return -1;
        }
    }

    public class Md5Hasher
    {
        public string Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5Hash.ComputeHash(bytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }
    }
}