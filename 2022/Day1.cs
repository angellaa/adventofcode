using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day1
{
    private readonly List<int> totalCaloriesByElf = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var totalCalories = 0;

        foreach (var line in File.ReadAllLines("Day1.txt"))
        {
            if (line == "")
            {
                totalCaloriesByElf.Add(totalCalories);
                totalCalories = 0;
                continue;
            }

            totalCalories += int.Parse(line);
        }
    }

    [Test]
    public void Part1() => Assert.That(totalCaloriesByElf.Max(), Is.EqualTo(75622));

    [Test]
    public void Part2() => Assert.That(totalCaloriesByElf.OrderDescending().Take(3).Sum(), Is.EqualTo(213159));
}