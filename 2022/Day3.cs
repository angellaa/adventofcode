using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day3
{
    private List<string> lines = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        lines = File.ReadAllLines("input.txt").ToList();
    }

    [Test]
    public void Part1()
    {
        var total = 0;

        foreach (var line in lines)
        {
            var a= line.Chunk(line.Length / 2).ToList()[0];
            var b = line.Chunk(line.Length / 2).ToList()[1];
            
            var s = new string(a.Where(x => b.Contains(x)).Distinct().ToArray());

            total += char.IsLower(s[0]) ? s[0] - 'a' + 1 : s[0] - 'A' + 27;
        }

        Assert.That(total, Is.EqualTo(8109));
    }

    [Test]
    public void Part2()
    {
        var total = 0;

        foreach (var chunk in lines.Chunk(3))
        {
            var s = chunk[0];

            foreach (var line in chunk)
            {
                s = (new string(line.Where(x => s.Contains(x)).Distinct().ToArray()));
            }

            if (s != "")
            {
                total += char.IsLower(s[0]) ? s[0] - 'a' + 1 : s[0] - 'A' + 27;
            }
        }

        Assert.That(total, Is.EqualTo(2738));
    }
}