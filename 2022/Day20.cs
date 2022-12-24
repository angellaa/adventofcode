using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day20
{
    private readonly List<(int value, int id)> numbers = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var lines = File.ReadAllLines("Input.txt").Select(int.Parse).ToList();

        for (var i = 0; i < lines.Count; i++)
        {
            numbers.Add((lines[i], i));
        }
    }

    [Test]
    public void Part1()
    {
        for (var id = 0; id < numbers.Count; id++)
        {
            var index = numbers.FindIndex(n => n.id == id);
            var value = numbers[index].value;

            if (value == 0) continue;

            var newIndex = (index + value) % (numbers.Count - 1);

            if (newIndex <= 0) newIndex += numbers.Count - 1;

            numbers.RemoveAt(index);
            numbers.Insert(newIndex, (value, id));
        }

        var zeroIndex = numbers.FindIndex(n => n.value == 0);

        var i1000 = (zeroIndex + 1000) % numbers.Count;
        var i2000 = (zeroIndex + 2000) % numbers.Count;
        var i3000 = (zeroIndex + 3000) % numbers.Count;

        var result = numbers[i1000].value + numbers[i2000].value + numbers[i3000].value;

        Assert.That(result, Is.EqualTo(10763));
    }

    [Test]
    public void Part2()
    {
    }
}