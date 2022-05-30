using System;
using System.Collections.Generic;
using System.IO;
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
        public void ComputeWire_WireToWire_AddsWire()
        {
            var subject = new Circuit();
            subject.WireExists("x").Should().BeFalse(); //precondition assertion
            subject.ComputeWire("123 -> x");

            subject.ComputeWire("x -> a");

            subject.WireExists("a").Should().BeTrue();
            subject.GetWireValue("a").Should().Be(123);
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

        [Test]
        public void ComputeWire_LiteralIsFirst_DoesntAddWire()
        {
            var subject = SetUpTestCircuit();

            subject.ComputeWire("1 AND cx -> cy");
            subject.WireExists("cy").Should().BeFalse("cx is not a valid parent");
        }

        [Test]
        public void ComputeAllWiresInFile_SampleInput_GetsSampleAnswer()
        {
            var subject = new Circuit();

            subject.ComputeAllWiresInFile(
                "C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\day-7-sample-input.txt");

            subject.GetWireValue("d").Should().Be(72, "that's a supported AND statement");
            subject.GetWireValue("e").Should().Be(507, "that's a supported OR statement");
            subject.GetWireValue("f").Should().Be(492, "that's a supported LSHIFT statement");
            subject.GetWireValue("g").Should().Be(114, "that's a supported RSHIFT statement");
            subject.GetWireValue("h").Should().Be(65412, "that's a supported NOT statement");
            subject.GetWireValue("i").Should().Be(65079, "that's a supported NOT statement");
            subject.GetWireValue("x").Should().Be(123, "it's part of setting up");
            subject.GetWireValue("y").Should().Be(456, "it's part of setting up");
        }

        [Test]
        public void ComputeAllWiresInFile_RealInputNoRetry_GetsAnswerMaybe()
        {
            var subject = new Circuit();

            subject.ComputeAllWiresInFile(
                "C:\\Projects\\Homework\\advent-of-code-2015\\Advent2015\\input-day7.txt");

            //subject.GetNumberOfWires().Should().Be(0);
            subject.GetWireValue("a").Should().Be(14134); //was 46065
        }

        [Test]
        public void ComputeWire_1And2_ResultIs3()
        {
            var subject = new Circuit();

            subject.ComputeWire("1 AND 3 -> a");

            subject.WireExists("a").Should()
                .BeTrue("because apparently this is valid even though the docs say it isn't");
            subject.GetWireValue("a").Should().Be(1);
        }

    }

    public class Circuit
    {
        private readonly Dictionary<string, ushort> _wires;

        public Circuit()
        {
            _wires = new Dictionary<string, ushort>();
        }
        
        public bool WireExists(string wire)
        {
            return _wires.ContainsKey(wire);
        }

        public bool ComputeWire(string command)
        {
            var tokens = SplitCommandIntoTokens(command);

            if (tokens[1] == "->")
            {
                if (ProcessLiteralAssignment(tokens)) return true;
            }
            else if (tokens[0] == "NOT")
            {
                return ProcessNot(tokens);
            }
            else
            {
                return ComputeGate(tokens);
            }

            return false;
        }

        private bool ProcessLiteralAssignment(List<string> tokens)
        {
            ushort result;
            if (ushort.TryParse(tokens[0], out result))
            {
                _wires.Add(tokens[2], result);
                return true;
            }
            else if (_wires.TryGetValue(tokens[0], out result))
            {
                _wires.Add(tokens[2], result);
                return true;
            }

            return false;
        }

        private bool ProcessNot(List<string> tokens)
        {
            var notParentExists = _wires.TryGetValue(tokens[1], out var lValue);
            if (!notParentExists) return false;
            _wires.Add(tokens[3], (ushort) ~lValue);
            return true;
        }

        private bool ComputeGate(List<string> tokens)
        {
            if (!int.TryParse(tokens[0], out _) && !_wires.ContainsKey(tokens[0]))
                return false;
            if (!int.TryParse(tokens[2], out _) && !_wires.ContainsKey(tokens[2]))
                return false; //can't compute a gate unless both parent wires or a literal is present

            
            if(!_wires.TryGetValue(tokens[0], out ushort lvalue)) // not in wires
            {
                lvalue = ushort.Parse(tokens[0]);
            }
            switch (tokens[1])
            {
                case "LSHIFT":
                    _wires.Add(tokens[4], (ushort)(lvalue << int.Parse(tokens[2])));
                    return true;
                case "RSHIFT":
                    _wires.Add(tokens[4], (ushort)(lvalue >> int.Parse(tokens[2])));
                    return true;
            }


            if (!_wires.TryGetValue(tokens[2], out ushort rvalue))
            {
                rvalue = ushort.Parse(tokens[2]);
            }
            switch (tokens[1])
            {
                case "AND":
                    _wires.Add(tokens[4], (ushort)(lvalue & rvalue));
                    return true;
                case "OR":
                    _wires.Add(tokens[4], (ushort)(lvalue | rvalue));
                    return true;
                default:
                    return false;
            }
        }

        private List<string> SplitCommandIntoTokens(string command)
        {
            return command.Split(new[]{' '}, StringSplitOptions.None).ToList();
        }

        public ushort GetWireValue(string wire)
        {
            _wires.TryGetValue(wire, out var result);
            return result;
        }

        public void ComputeAllWiresInFile(string textFile)
        {
            var commands = File.ReadAllLines(textFile).ToList();
            while (commands.Count > 0)
            {
                var commandsToRemove = new List<string>();
                foreach (var command in commands)
                {
                    var succeeded = ComputeWire(command);
                    if (succeeded)
                    {
                        commandsToRemove.Add(command);
                    }
                }

                foreach (var commandToRemove in commandsToRemove)
                {
                    commands.Remove(commandToRemove);
                }
            }
        }

        public int GetNumberOfWires()
        {
            return _wires.Count;
        }
    }
}