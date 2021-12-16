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
    }

    [Test]
    public void Part1()
    {
        var packets = Parse(bits).Packets;
        
        Assert.That(Sum(packets), Is.EqualTo(0));
    }

    public int Sum(List<Packet> packets)
    {
        var sum = 0;

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

    public static (List<Packet> Packets, string Unprocessed) Parse(string bits, int packets = -1)
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
                Value = Convert.ToInt32(literal, 2)
            };

            result.Add(literalPacket);

            var rest = bits.Take(start..);

            result.AddRange(Parse(rest, packets - 1).Packets);
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
                    SubPackets = Parse(bits.Take(22..(22 + lengthsInBits))).Packets
                };

                result.Add(packet);

                var rest = bits.Take((22 + lengthsInBits)..);

                result.AddRange(Parse(rest, packets - 1).Packets);

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

                result.AddRange(Parse(parsed.Unprocessed).Packets);
            }
        }

        return (result, "");
    }

    public class Packet
    {
        public int Version { get; init; }
        public int TypeId { get; init; }
    }

    public class OperatorPacket : Packet
    {
        public List<Packet> SubPackets { get; init; } = new();
    }

    public class LiteralPacket : Packet
    {
        public int Value { get; init; }
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
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