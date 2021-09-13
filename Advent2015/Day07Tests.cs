using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day07Tests
    {
        [Test]
        public void WireExists_NoWire_ReturnsFalse()
        {
            var subject = new Circuit();
            bool result = subject.WireExists("aa");
            result.Should().BeFalse("No wires have been defined yet");
        }
    }

    public class Circuit
    {
        private Dictionary<string, Int16> _wires;

        public Circuit()
        {
            _wires = new Dictionary<string, short>();
        }
        
        public bool WireExists(string wire)
        {
            return _wires.ContainsKey(wire);
        }
    }
}