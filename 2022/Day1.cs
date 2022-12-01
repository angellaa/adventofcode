using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day1
{
    private List<string> lines;

    [SetUp]
    public void SetUp()
    {
        lines = File.ReadAllLines("Day1.txt").ToList();
    }

    [Test]
    public void Part1()
    {
        long max = long.MinValue;
        long v = 0;
        foreach (var line in lines)
        {
            if (line == "")
            {
                if (v > max) max = v;
                v = 0;
                continue;
            }

            v += long.Parse(line);
        }

        if (v > max) max = v;

        Assert.That(max, Is.EqualTo(75622));
    }

    [Test]
    public void Part2()
    {
        var list = new List<long>();
        long v = 0;

        foreach (var line in lines)
        {
            if (line == "")
            {
                list.Add(v);
                v = 0;
                continue;
            }

            v += long.Parse(line);
        }

        Assert.That(list.OrderByDescending(x => x).Take(3).Sum(), Is.EqualTo(213159));
    }
}