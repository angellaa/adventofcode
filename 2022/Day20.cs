using System.Numerics;
using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day20
{
    private List<(BigInteger value, int id)> numbers;

    [SetUp]
    public void SetUp()
    {
        numbers = new List<(BigInteger value, int id)>();

        var lines = File.ReadAllLines("Input.txt").Select(int.Parse).ToList();

        for (var i = 0; i < lines.Count; i++)
        {
            numbers.Add((lines[i], i));
        }
    }

    [Test]
    public void Part1()
    {
        Mix();

        Assert.That(GetDecryptionKey(), Is.EqualTo(new BigInteger(10763)));
    }

    [Test]
    public void Part2()
    {
        var decryptionKey = new BigInteger(811589153);

        for (var i = 0; i < numbers.Count; i++)
        {
            numbers[i] = (numbers[i].value * decryptionKey, numbers[i].id);
        }

        for (var i = 0; i < 10; i++)
        {
            Mix();
        }

        Assert.That(GetDecryptionKey(), Is.EqualTo(new BigInteger(0)));
    }

    private void Mix()
    {
        for (var id = 0; id < numbers.Count; id++)
        {
            var index = numbers.FindIndex(n => n.id == id);
            var value = numbers[index].value;

            if (value == 0) continue;

            var newIndex = (int)((index + value) % (numbers.Count - 1));

            if (newIndex <= 0) newIndex += numbers.Count - 1;

            numbers.RemoveAt(index);
            numbers.Insert(newIndex, (value, id));
        }
    }

    private BigInteger GetDecryptionKey()
    {
        var zeroIndex = numbers.FindIndex(n => n.value == 0);

        var i1000 = (zeroIndex + 1000) % numbers.Count;
        var i2000 = (zeroIndex + 2000) % numbers.Count;
        var i3000 = (zeroIndex + 3000) % numbers.Count;

        return numbers[i1000].value + numbers[i2000].value + numbers[i3000].value;
    }
}