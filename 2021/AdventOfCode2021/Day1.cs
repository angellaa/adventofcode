using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day1
{
    private List<int> depths;

    [SetUp]
    public void SetUp()
    {
        depths = File.ReadAllLines("Day1.txt").Select(x => int.Parse(x)).ToList();
    }

    [Test]
    public void Part1()
    {
        var result = 0;

        for (int i = 1; i < depths.Count; i++)
        {
            if (depths[i] > depths[i - 1]) result++;
        }

        Assert.That(result, Is.EqualTo(1676));
    }

    [Test]
    public void Part2()
    {
        var result = 0;

        for (int i = 3; i < depths.Count; i++)
        {
            if (depths[i] + depths[i - 1] + depths[i - 2] > depths[i - 1] + depths[i - 2] + depths[i - 3])
            {
                result++;
            }
        }

        Assert.That(result, Is.EqualTo(1706));
    }
}