using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day5
{
    private List<(int X1, int Y1, int X2, int Y2)> vents;

    [SetUp]
    public void SetUp()
    {
        vents = File.ReadAllLines("Day5.txt")
            .Select(vent => 
                (int.Parse(vent.Split(" -> ")[0].Split(",")[0]),
                 int.Parse(vent.Split(" -> ")[0].Split(",")[1]),
                 int.Parse(vent.Split(" -> ")[1].Split(",")[0]),
                 int.Parse(vent.Split(" -> ")[1].Split(",")[1])))
            .ToList();
    }

    [Test]
    public void Part1()
    {
        var points = new int[1000, 1000];

        foreach (var (x1, y1, x2, y2) in vents)
        {
            if (x1 == x2) for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++) points[x1, y]++;
            if (y1 == y2) for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++) points[x, y1]++;
        }

        var overlapsCount = points.Cast<int>().Count(p => p >= 2);

        Assert.That(overlapsCount, Is.EqualTo(6397));
    }

    [Test]
    public void Part2()
    {
        var points = new int[1000, 1000];

        foreach (var (x1, y1, x2, y2) in vents)
        {
            var (x, y) = (x1, y1);
            var (dx, dy) = (Math.Sign(x2 - x1), Math.Sign(y2 - y1));

            while ((x, y) != (x2, y2))
            {
                points[x, y]++;
                x += dx;
                y += dy;
            }

            points[x, y]++;
        }

        var overlapsCount = points.Cast<int>().Count(p => p >= 2);

        Assert.That(overlapsCount, Is.EqualTo(22335));
    }
}