using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day7
{
    private List<int> positions;

    [SetUp]
    public void SetUp()
    {
        positions = File.ReadAllText("Day7.txt").Split(",").Select(int.Parse).ToList();
    }

    [Test]
    public void Part1()
    {
        var minFuelCost = int.MaxValue;

        for (var i = 0; i <= positions.Max(); i++)
        {
            var alignmentCost = positions.Sum(x => Math.Abs(i - x));

            minFuelCost = Math.Min(alignmentCost, minFuelCost);
        }

        Assert.That(minFuelCost, Is.EqualTo(352997));
    }

    [Test]
    public void Part2()
    {
        var minFuelCost = int.MaxValue;

        for (var i = 0; i <= positions.Max(); i++)
        {
            var alignmentCost = positions.Sum(x => Math.Abs(i - x) * (Math.Abs(i - x) + 1) / 2);

            minFuelCost = Math.Min(alignmentCost, minFuelCost);
        }

        Assert.That(minFuelCost, Is.EqualTo(101571302));            
    }
}