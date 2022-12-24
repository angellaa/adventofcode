using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day24
{
    private List<char[]> map = new();
    private HashSet<(int y, int x)> spaces = new();
    private readonly Dictionary<int, HashSet<(int y, int s)>> spacesByMinute = new();
    private readonly HashSet<(int y, int x)> allSpaces = new();
    private readonly List<(int y, int x, char dir)> blizzards = new();
    private readonly Dictionary<(int y, int x), int> visited = new();
    private int n;
    private int m;
    private readonly int maxMinute = 313;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        map = File.ReadAllLines("Input.txt").Select(x => x.ToArray()).ToList();
        n = map.Count;
        m = map[0].Length;

        for (var y = 0; y < n; y++)
        for (var x = 0; x < m; x++)
        {
            var dir = map[y][x];
            if (dir is '^' or 'v' or '>' or '<') blizzards.Add((y, x, dir));
            
            visited[(y, x)] = 0;
        }

        for (var y = 1; y < n - 1; y++)
        for (var x = 1; x < m - 1; x++)
        {
            allSpaces.Add((y, x));
        }

        allSpaces.Add((0, 1));
        allSpaces.Add((n - 1, m - 2));
        spaces = allSpaces.Except(blizzards.Select(b => (b.y, b.x))).ToHashSet();

        spacesByMinute[0] = spaces;
    }

    [Test]
    public void Part1()
    {
        //Print();

        for (var minute = 1; minute <= maxMinute; minute++)
        {
            Move();
            spacesByMinute[minute] = allSpaces.Except(blizzards.Select(b => (b.y, b.x))).ToHashSet();

            //Console.WriteLine("Minute " + minute);
            //Print();
        }

        Assert.That(Count(0, 1, 0, 0, 0), Is.EqualTo(18));
    }

    private int Count(int y, int x, int minute, int waits, int backSteps)
    {
        if (visited[(y, x)] > 2) return int.MaxValue;
        if (minute + (n - 1 - y) + (m - 2 - x) >= maxMinute) return int.MaxValue;
        if (waits > 2) return int.MaxValue;
        if (backSteps > 2) return int.MaxValue;

        visited[(y, x)]++;

        //if (y == n - 1 && x == m - 2)
        //{
        //    Debug.WriteLine("Solution found at minute " + minute);
        //    return minute;
        //}

        var nextSpace = spacesByMinute[minute + 1];

        var min = int.MaxValue;

        if (nextSpace.Contains((y, x + 1))) min = Math.Min(min, Count(y, x + 1, minute + 1, 0, 0));
        if (nextSpace.Contains((y + 1, x))) min = Math.Min(min, Count(y + 1, x, minute + 1, 0, 0));
        if (nextSpace.Contains((y, x))) min = Math.Min(min, Count(y, x, minute + 1, waits + 1, 0));
        if (nextSpace.Contains((y, x - 1))) min = Math.Min(min, Count(y, x - 1, minute + 1, 0, backSteps + 1));
        if (nextSpace.Contains((y - 1, x))) min = Math.Min(min, Count(y - 1, x, minute + 1, 0, backSteps + 1));

        visited[(y, x)]--;

        return min;
    }

    private void Move()
    {
        for (var i = 0; i < blizzards.Count; i++)
        {
            var b = blizzards[i];

            if (b.dir == '<')
            {
                var newX = (m - 2 + (b.x - 1) - 1) % (m - 2) + 1;
                b = (b.y, newX, b.dir);
            }

            if (b.dir == '>')
            {
                var newX = ((b.x - 1) + 1) % (m - 2) + 1;
                b = (b.y, newX, b.dir);
            }

            if (b.dir == '^')
            {
                var newY = (n - 2 + (b.y - 1) - 1) % (n - 2) + 1;
                b = (newY, b.x, b.dir);
            }

            if (b.dir == 'v')
            {
                var newY = ((b.y - 1) + 1) % (n - 2) + 1;
                b = (newY, b.x, b.dir);
            }

            blizzards[i] = b;
        }

        map = new List<char[]>();

        map.Add(("#." + new string('#', m - 2)).ToArray());

        for (var y = 0; y < n - 2; y++)
        {
            map.Add(("#" + new string('.', m - 2) + "#").ToArray());
        }

        map.Add(("#" + new string('#', m - 3) + ".#").ToArray());

        foreach (var b in blizzards)
        {
            map[b.y][b.x] = b.dir;
        }
    }

    private void Print()
    {
        foreach (var c in map)
        {
            Console.WriteLine(new string(c));
        }

        Console.WriteLine();
    }

    [Test]
    public void Part2()
    {
    }
}