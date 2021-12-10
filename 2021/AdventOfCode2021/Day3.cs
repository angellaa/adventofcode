using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day3
{
    List<string> binaryNumbers;
    int numberOfBits;

    [SetUp]
    public void SetUp()
    {
        binaryNumbers = File.ReadAllLines("Day3.txt").ToList();
        numberOfBits = binaryNumbers[0].Length;
    }

    [Test]
    public void Part1()
    {
        var gammaRate = new string(Enumerable.Range(0, numberOfBits)
            .Select(i => binaryNumbers.Count(c => c[i] == '1') > 
                         binaryNumbers.Count(c => c[i] == '0') ? '1' : '0')
            .ToArray());

        var epsilonRate = new string(gammaRate.Select(x => x == '1' ? '0' : '1')
            .ToArray());

        var powerConsumption = Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);

        Assert.That(powerConsumption, Is.EqualTo(3912944));
    }

    [Test]
    public void Part2()
    {
        var oxygenGeneratorRating = GetOxygenGeneratorRating();
        var co2ScrubberRating = GetCo2ScrubberRating();

        var lifeSupportRating = oxygenGeneratorRating * co2ScrubberRating;

        Assert.That(lifeSupportRating, Is.EqualTo(4996233));
    }

    private int GetOxygenGeneratorRating()
    {
        var numbers = binaryNumbers.ToList();

        for (var i = 0; i < numberOfBits; i++)
        {
            var mostCommonValue = numbers.Count(c => c[i] == '1') >= numbers.Count(c => c[i] == '0') ? '1' : '0';

            numbers.RemoveAll(x => x[i] != mostCommonValue);

            if (numbers.Count == 1) break;
        }

        return Convert.ToInt32(numbers.First(), 2);
    }

    private int GetCo2ScrubberRating()
    {
        var numbers = binaryNumbers.ToList();

        for (var i = 0; i < numberOfBits; i++)
        {
            var leastCommonValue = numbers.Count(c => c[i] == '1') < numbers.Count(c => c[i] == '0') ? '1' : '0';

            numbers.RemoveAll(x => x[i] != leastCommonValue);

            if (numbers.Count == 1) break;
        }

        return Convert.ToInt32(numbers.First(), 2);
    }
}