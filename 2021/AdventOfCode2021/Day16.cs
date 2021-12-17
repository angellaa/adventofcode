using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day16
{
    string bits;

    [SetUp]
    public void SetUp()
    {
        bits = string.Join("", File.ReadAllText("Day16.txt")
                                    .Select(x => Convert.ToString(Convert.ToInt32(x.ToString(), 16), 2).PadLeft(4, '0')));

        Console.WriteLine(bits);
    }

    [Test]
    public void Part1()
    {
        var packets = Parse(bits, -1).Packets;

        Print(packets);
        
        Assert.That(Sum(packets), Is.EqualTo(971));
    }

    public void Print(List<Packet> packets, int depth = 0)
    {
        foreach (var packet in packets)
        {
            if (packet is LiteralPacket literal)
            {
                Console.WriteLine(new string(' ', 2 * depth) + "Literal Version " + literal.Version + " = " + literal.Value);
            }
            else if (packet is OperatorPacket op)
            {
                Console.WriteLine(new string(' ', 2 * depth) + "Operator Version " + op.Version);
                Print(op.SubPackets, depth + 1);
            }
        }
    }
    
    public long Sum(List<Packet> packets)
    {
        var sum = 0L;

        foreach (var packet in packets)
        {
            sum += packet.Version;

            if (packet is OperatorPacket o)
            {
                sum += Sum(o.SubPackets);
            }
        }

        return sum;
    }

    public static (List<Packet> Packets, string Unprocessed) Parse(string bits, int packets)
    {
        if (bits.Replace("0", "") == "" || packets == 0)
        {
            return (new List<Packet>(), bits);
        }

        var result = new List<Packet>();

        var version = bits.TakeInt(..3);
        var typeId = bits.TakeInt(3..6);
        
        if (typeId == 4)
        {
            var start = 6;
            var literal = "";
            string part;

            do
            {
                part = bits.Take(start..(start + 5));
                literal += part.Take(1..);
                start += 5;
            } 
            while (part[0] == '1');

            var literalPacket = new LiteralPacket
            {
                Version = version,
                TypeId = typeId,
                Value = Convert.ToInt64(literal, 2)
            };

            result.Add(literalPacket);

            var rest = bits.Take(start..);

            var (nextTypes, unprocessed) = Parse(rest, packets - 1);

            result.AddRange(nextTypes);

            return (result, unprocessed);
        }
        else
        {
            var lengthTypeID = bits.TakeInt(6..7);
            if (lengthTypeID == 0)
            {
                var lengthsInBits = bits.TakeInt(7..22);

                var packet = new OperatorPacket
                {
                    Version = version,
                    TypeId = typeId,
                    SubPackets = Parse(bits.Take(22..(22 + lengthsInBits)), -1).Packets
                };

                result.Add(packet);

                var rest = bits.Take((22 + lengthsInBits)..);

                var (subTypes, unprocessed) = Parse(rest, packets - 1);

                result.AddRange(subTypes);

                return (result, unprocessed);
            }
            else
            {
                var subPacketsNumber = bits.TakeInt(7..18);

                var parsed = Parse(bits.Take(18..), subPacketsNumber);

                var packet = new OperatorPacket
                {
                    Version = version,
                    TypeId = typeId,
                    SubPackets = parsed.Packets
                };

                result.Add(packet);

                var (subTypes, unprocessed) = Parse(parsed.Unprocessed, packets - 1);

                result.AddRange(subTypes);

                return (result, unprocessed);
            }
        }
    }

    public class Packet
    {
        public int Version { get; init; }
        public int TypeId { get; init; }
        public virtual long Value { get; init; }
    }

    public class OperatorPacket : Packet
    {
        public List<Packet> SubPackets { get; init; } = new();

        public override long Value => TypeId switch
                {
                    0 => SubPackets.Sum(x => x.Value),
                    1 => SubPackets.Aggregate(1L, (a, y) => a * y.Value),
                    2 => SubPackets.Min(x => x.Value),
                    3 => SubPackets.Max(x => x.Value),
                    5 => SubPackets[0].Value > SubPackets[1].Value ? 1 : 0,
                    6 => SubPackets[0].Value < SubPackets[1].Value ? 1 : 0,
                    7 => SubPackets[0].Value == SubPackets[1].Value ? 1 : 0,
                };
    }

    public class LiteralPacket : Packet
    {
    }

    [Test]
    public void Part2()
    {
        var packets = Parse(bits, -1).Packets;
        
        Assert.That(packets[0].Value, Is.EqualTo(0));
    }
}

public static class StringEx
{
    public static string Take(this string s, Range n)
    {
        return new string(s[n]);
    }

    public static int TakeInt(this string s, Range n)
    {
        return Convert.ToInt32(s.Take(n), 2);
    }
}