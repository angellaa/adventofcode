using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day6
{
    private List<int> ages;

    [SetUp]
    public void SetUp()
    {
        ages = File.ReadAllText("Day6.txt").Split(",").Select(int.Parse).ToList();
    }

    [Test]
    public void Part1()
    {
        for (var i = 0; i < 80; i++)
        {
            var newFishes = 0;

            for (var j = 0; j < ages.Count; j++)
            {
                if (ages[j] > 0) ages[j]--;
                else
                {
                    ages[j] = 6;
                    newFishes++;
                }
            }

            ages.AddRange(Enumerable.Repeat(8, newFishes));
        }

        Assert.That(ages.Count, Is.EqualTo(366057));
    }

    [Test]
    public void Part2()
    {
        const int n = 10;
        var fishCountByAge = new long[n];

        foreach (var age in ages)
        {
            fishCountByAge[age]++;
        }

        var zeroAgeIndex = 0;

        for (var i = 0; i < 256; i++)
        {
            fishCountByAge[(zeroAgeIndex + 9) % n] += fishCountByAge[zeroAgeIndex];
            fishCountByAge[(zeroAgeIndex + 7) % n] += fishCountByAge[zeroAgeIndex];
            fishCountByAge[zeroAgeIndex] = 0;

            zeroAgeIndex = (zeroAgeIndex + 1) % n;
        }

        Assert.That(fishCountByAge.Sum(), Is.EqualTo(1653559299811));
    }
}