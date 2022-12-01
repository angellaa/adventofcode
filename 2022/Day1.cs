using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day1
{
    private List<int> totalCaloriesByElf;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        totalCaloriesByElf = File.ReadAllLines("Day1.txt")
                                 .ChunkBy(x => x == "")
                                 .Select(x => x.Select(int.Parse).Sum())
                                 .ToList();
    }

    [Test]
    public void Part1() => Assert.That(totalCaloriesByElf.Max(), Is.EqualTo(75622));

    [Test]
    public void Part2() => Assert.That(totalCaloriesByElf.OrderDescending().Take(3).Sum(), 
                               Is.EqualTo(213159));

}