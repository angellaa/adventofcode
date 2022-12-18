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
        var result = 0;

        foreach (var (x, y, z) in cubes)
        {
            var adjacents = 0;

            if (cubes.Contains((x - 1, y, z)) || Trapped((x - 1, y, z))) adjacents++;
            if (cubes.Contains((x + 0, y, z)) || Trapped((x + 0, y, z))) adjacents++;
            if (cubes.Contains((x + 1, y, z)) || Trapped((x + 1, y, z))) adjacents++;
            if (cubes.Contains((x, y - 1, z)) || Trapped((x, y - 1, z))) adjacents++;
            if (cubes.Contains((x, y + 0, z)) || Trapped((x, y + 0, z))) adjacents++;
            if (cubes.Contains((x, y + 1, z)) || Trapped((x, y + 1, z))) adjacents++;
            if (cubes.Contains((x, y, z - 1)) || Trapped((x, y, z - 1))) adjacents++;
            if (cubes.Contains((x, y, z + 0)) || Trapped((x, y, z + 0))) adjacents++;
            if (cubes.Contains((x, y, z + 1)) || Trapped((x, y, z + 1))) adjacents++;

            result += 9 - adjacents;
        }

        Assert.That(result, Is.EqualTo(2118));
    }

    public bool Trapped((int, int, int) cube)
    {
        var set = new Stack<(int, int, int)>();
        var visited = new HashSet<(int, int, int)>();
        set.Push(cube);

        while (set.Count > 0)
        {
            var (x, y, z) = set.Pop();

            if (visited.Contains((x, y, z))) continue;

            if (x < 0 || y < 0 || z < 0 || x > 20 || y > 20 || z > 20)
            {
                return false;
            }

            visited.Add((x, y, z));

            if (!cubes.Contains((x - 1, y, z))) set.Push((x - 1, y, z));
            if (!cubes.Contains((x + 0, y, z))) set.Push((x + 0, y, z));
            if (!cubes.Contains((x + 1, y, z))) set.Push((x + 1, y, z));
            if (!cubes.Contains((x, y - 1, z))) set.Push((x, y - 1, z));
            if (!cubes.Contains((x, y + 0, z))) set.Push((x, y + 0, z));
            if (!cubes.Contains((x, y + 1, z))) set.Push((x, y + 1, z));
            if (!cubes.Contains((x, y, z - 1))) set.Push((x, y, z - 1));
            if (!cubes.Contains((x, y, z + 0))) set.Push((x, y, z + 0));
            if (!cubes.Contains((x, y, z + 1))) set.Push((x, y, z + 1));
        }

        return true;
    }
}