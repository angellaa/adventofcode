using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day7
{
    List<int> input;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllText("Day7.txt").Split(",").Select(x => int.Parse(x)).ToList();
    }

    [Test]
    public void Part1()
    {
        var max = input.Max();
        var min = int.MaxValue;

        for (int i = 0; i < max; i++)
        {
            min = Math.Min(input.Sum(x => Math.Abs(i - x)), min);
        }

        Assert.That(min, Is.EqualTo(352997));
    }

    [Test]
    public void Part2()
    {
        var max = input.Max();
        var min = int.MaxValue;

        for (int i = 0; i < max; i++)
        {
            min = Math.Min(input.Sum(x => Math.Abs(i - x) * (Math.Abs(i - x) + 1) / 2), min);
        }

        Assert.That(min, Is.EqualTo(101571302));            
    }
}