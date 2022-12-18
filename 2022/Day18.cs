using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day18
{
    private HashSet<(int, int, int)> cubes;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        cubes = File.ReadAllLines("Input.txt")
                    .Select(x => x.Split(",").ToArray())
                    .Select(x => (int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
                    .ToHashSet();
    }

    [Test]
    public void Part1()
    {
        var result = 0;

        foreach (var (x, y, z) in cubes)
        {
            var adjacents = 0;

            if (cubes.Contains((x - 1, y, z))) adjacents++;
            if (cubes.Contains((x + 0, y, z))) adjacents++;
            if (cubes.Contains((x + 1, y, z))) adjacents++;
            if (cubes.Contains((x , y - 1, z))) adjacents++;
            if (cubes.Contains((x , y + 0, z))) adjacents++;
            if (cubes.Contains((x , y + 1, z))) adjacents++;
            if (cubes.Contains((x , y, z - 1))) adjacents++;
            if (cubes.Contains((x , y, z + 0))) adjacents++;
            if (cubes.Contains((x , y, z + 1))) adjacents++;

            result += 9 - adjacents;
        }

        Assert.That(result, Is.EqualTo(3650));
    }

    [Test]
    public void Part2()
    {
    }
}