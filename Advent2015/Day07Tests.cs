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
        public void ComputeWire_Not_ComputesBitwiseNot()
        {
            //123 -> x
            var subject = new Circuit();
            subject.ComputeWire("123 -> x");
            subject.WireExists("x").Should().BeTrue("the wire was added"); //precondition assertion
            
            subject.ComputeWire("NOT x -> h");

            subject.WireExists("h").Should().BeTrue("the wire was added");
            subject.GetWireValue("h").Should().Be(65412);
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
        
        [Test]
        public void ComputeWire_LeftShiftNoParent_DoesntAdd()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("a LSHIFT 2 -> z");
            subject.WireExists("z").Should().BeFalse("a doesn't exist");
        }
        
        [Test]
        public void ComputeWire_LeftShiftWithParent_AddsWire()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("x LSHIFT 2 -> z");
            subject.WireExists("z").Should().BeTrue("it has a valid parent and a value");
            subject.GetWireValue("z").Should().Be(492, "That's an LSHIFT of 2");
        }
        
        [Test]
        public void ComputeWire_RightShiftWithParent_AddsWire()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("y RSHIFT 2 -> z");
            subject.WireExists("z").Should().BeTrue("it has a valid parent and a value");
            subject.GetWireValue("z").Should().Be(114, "That's an RSHIFT of 2");
        }
        
        [Test]
        public void ComputeWire_NotWithNoParent_DoesntAdd()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("NOT a -> z");
            subject.WireExists("z").Should().BeFalse("a doesn't exist");
        }
        
        [Test]
        public void ComputeWire_NotWithParent_AddsWire()
        {
            var subject = SetUpTestCircuit();
            
            subject.ComputeWire("NOT x -> z");
            subject.WireExists("z").Should().BeTrue("x is a valid parent");
            subject.GetWireValue("z").Should().Be(65412, "that's a NOT of x");
        }

    }

    public class Circuit
    {
        private Dictionary<string, ushort> _wires;

        public Circuit()
        {
            _wires = new Dictionary<string, ushort>();
        }
        
        public bool WireExists(string wire)
        {
            return _wires.ContainsKey(wire);
        }

        public void ComputeWire(string command)
        {
            var tokens = SplitCommandIntoTokens(command);
            var couldParse = ushort.TryParse(tokens[0], out var result);
            if (couldParse)
            {
                _wires.Add(tokens[2], result);
            }
            else if (tokens[0] == "NOT")
            {
                var notParentExists = _wires.TryGetValue(tokens[1], out var lValue);
                if (notParentExists)
                {
                    _wires.Add(tokens[3], (ushort) ~lValue);
                }
            }
            else
            {
                ComputeGate(tokens);
            }
        }

        private void ComputeGate(List<string> tokens)
        {
            if (!_wires.ContainsKey(tokens[0]))
                return;
            if (!int.TryParse(tokens[2], out int garbage) && !_wires.ContainsKey(tokens[2]))
                return; //can't compute a gate unless both parent wires are present
            
            _wires.TryGetValue(tokens[0], out ushort lvalue);
            if (tokens[1] == "LSHIFT")
            {
                _wires.Add(tokens[4], (ushort)(lvalue << int.Parse(tokens[2])));
                return;
            }
            if (tokens[1] == "RSHIFT")
            {
                _wires.Add(tokens[4], (ushort)(lvalue >> int.Parse(tokens[2])));
                return;
            }

            
            _wires.TryGetValue(tokens[2], out ushort rvalue);
            if (tokens[1] == "AND")
            {
                _wires.Add(tokens[4], (ushort)(lvalue & rvalue));
                return;
            }
            if (tokens[1] == "OR")
            {
                _wires.Add(tokens[4], (ushort)(lvalue | rvalue));
                return;
            }
        }

        private List<string> SplitCommandIntoTokens(string command)
        {
            return command.Split(new char[]{' '}, StringSplitOptions.None).ToList();
        }

        public ushort GetWireValue(string wire)
        {
            _wires.TryGetValue(wire, out var result);
            return result;
        }
    }
}