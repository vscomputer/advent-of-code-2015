using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2015
{
    [TestFixture]
    public class Day07Tests
    {
        private static Circuit SetUpTestCircuit()
        {
            var subject = new Circuit();
            subject.WireExists("x").Should().BeFalse(); //precondition assertion
            subject.WireExists("y").Should().BeFalse(); //precondition assertion
            subject.ComputeWire("123 -> x");
            subject.ComputeWire("456 -> y");
            return subject;
        }
        
        [Test]
        public void WireExists_NoWire_ReturnsFalse()
        {
            var subject = new Circuit();
            bool result = subject.WireExists("aa");
            result.Should().BeFalse("No wires have been defined yet");
        }

        [Test]
        public void ComputeWire_Simple_AddsWireAndValue()
        {
            //123 -> x
            var subject = new Circuit();
            subject.WireExists("x").Should().BeFalse(); //precondition assertion
            subject.ComputeWire("123 -> x");
            
            subject.WireExists("x").Should().BeTrue("the wire was added");
            subject.GetWireValue("x").Should().Be(123);
        }

        [Test]
        public void ComputeWire_NoParentWireTwo_DoesntAddWire()
        {
            var subject = new Circuit();
            subject.WireExists("x").Should().BeFalse(); //precondition assertion
            subject.ComputeWire("123 -> x");
            
            subject.ComputeWire("x AND y -> z");
            subject.WireExists("z").Should().BeFalse("y doesn't exist so it cannot be computed yet");
        }

        [Test]
        public void ComputerWire_AddWithTwoParents_AddsWire()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("x AND y -> z");
            subject.WireExists("z").Should().BeTrue("it has two valid parents");
            subject.GetWireValue("z").Should().Be(72, "because that's a bitwise AND of its parents");
        }
        
        [Test]
        public void ComputerWire_OrWithTwoParents_AddsWire()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("x OR y -> z");
            subject.WireExists("z").Should().BeTrue("it has two valid parents");
            subject.GetWireValue("z").Should().Be(507, "because that's a bitwise OR of its parents");
        }

    }

    public class Circuit
    {
        private Dictionary<string, int> _wires;

        public Circuit()
        {
            _wires = new Dictionary<string, int>();
        }
        
        public bool WireExists(string wire)
        {
            return _wires.ContainsKey(wire);
        }

        public void ComputeWire(string wire)
        {
            
            var splitter = new[] {"->"};
            var tokens = wire.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            
            if (_wires.ContainsKey(tokens[1]))
                return;

            if (tokens[0].Contains("AND"))
            {
                ComputeBinary(tokens, "AND");
            }
            else if (tokens[0].Contains("OR"))
            {
                ComputeBinary(tokens, "OR");
            }
            else
            {
                ComputeSimpleWire(tokens);
            }
        }

        private void ComputeBinary(string[] tokens, string op)
        {
            var wireKey = tokens[1].Trim();
            var splitter = new[] {op};
            var parentWire = tokens[0].Split(splitter, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();
            
            if (_wires.ContainsKey(parentWire[0]) == false || _wires.ContainsKey(parentWire[1]) == false)
            {
                return;
            }

            if (op == "AND")
            {
                _wires.Add(wireKey, _wires[parentWire[0]] & _wires[parentWire[1]]);
            }
            else if (op == "OR")
            {
                _wires.Add(wireKey, _wires[parentWire[0]] | _wires[parentWire[1]]);
            }
        }

        private void ComputeSimpleWire(string[] tokens)
        {
            var wireKey = tokens[1].Trim();
            int.TryParse(tokens[0], out var wireVal);
            _wires.Add(wireKey, wireVal);
        }

        public int GetWireValue(string wire)
        {
            if (WireExists(wire))
            {
                return _wires[wire];
            }
            else
            {
                throw new ArgumentException("no such key");
            }
        }
    }
}