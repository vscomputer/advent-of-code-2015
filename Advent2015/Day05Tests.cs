using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day05Tests
    {
        [Test]
        public void Reject_NicePair_NotRejected()
        {
            var subject = new EvaluatesChild();
            bool result = subject.AllPairsAreNice("aa");
        }
    }

    public class EvaluatesChild
    {
        public bool AllPairsAreNice(string input)
        {
            throw new System.NotImplementedException();
        }
    }
}