using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day1
{
    private List<int> totalCaloriesByElf = new List<int> { 1 };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var sequence = new[] { "1", "2", "", "3", "4", "", "5" };

        foreach (var chunk in sequence.ChunkBy(x => x == ""))
        {
            Console.WriteLine(string.Join(',', chunk));
        }
    }

    [Test]
    public void Part1() => Assert.That(totalCaloriesByElf.Max(), Is.EqualTo(75622));

    [Test]
    public void Part2() => Assert.That(totalCaloriesByElf.OrderDescending().Take(3).Sum(),
                               Is.EqualTo(213159));

}