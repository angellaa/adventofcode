using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day24
{
    private readonly Dictionary<int, HashSet<(int y, int s)>> spacesByMinute = new();
    private readonly List<(int y, int x, char dir)> blizzards = new();
    private int n;
    private int m;
    private const int maxMinutes = 900;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var map = File.ReadAllLines("Input.txt").Select(x => x.ToArray()).ToList();
        n = map.Count;
        m = map[0].Length;

        for (var y = 0; y < n; y++)
        for (var x = 0; x < m; x++)
        {
            var dir = map[y][x];
            if (dir is '^' or 'v' or '>' or '<') blizzards.Add((y, x, dir));
        }

        HashSet<(int y, int x)> allSpaces = new();

        for (var y = 1; y < n - 1; y++)
        for (var x = 1; x < m - 1; x++)
        {
            allSpaces.Add((y, x));
        }

        allSpaces.Add((0, 1));
        allSpaces.Add((n - 1, m - 2));

        spacesByMinute[0] = allSpaces.Except(blizzards.Select(b => (b.y, b.x))).ToHashSet();

        for (var minute = 1; minute <= maxMinutes; minute++)
        {
            Move();
            spacesByMinute[minute] = allSpaces.Except(blizzards.Select(b => (b.y, b.x))).ToHashSet();
        }
    }

    [Test]
    public void Part1()
    {
        Assert.That(Count((0, 1, 0), (n - 1, m - 2)), Is.EqualTo(301));
    }

    [Test]
    public void Part2()
    {
        var minutes1 = Count((0, 1, 0), (n - 1, m - 2));
        var minutes2 = Count((n - 1, m - 2, minutes1), (0, 1));
        var result = Count((0, 1, minutes2), (n - 1, m - 2));

        Assert.That(result, Is.EqualTo(859));
    }

    private int Count((int y, int x, int minute) root, (int y, int x) target)
    {
        var explored = new HashSet<(int y, int x, int minute)> { root };

        var queue = new Queue<(int y, int x, int minute)>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var (y, x, minute) = queue.Dequeue();

            if ((y, x) == target) return minute;

            var nextMoves = new (int y, int x, int minute)[]
            {
                (y, x + 1, minute + 1),
                (y, x - 1, minute + 1),
                (y, x, minute + 1),
                (y + 1, x, minute + 1),
                (y - 1, x, minute + 1)
            };

            foreach (var p in nextMoves)
            {
                if (spacesByMinute[p.minute].Contains((p.y, p.x)) && !explored.Contains(p))
                {
                    explored.Add(p); 
                    queue.Enqueue(p);
                }
            }
        }

        return int.MaxValue;
    }

    private void Move()
    {
        for (var i = 0; i < blizzards.Count; i++)
        {
            var b = blizzards[i];

            if (b.dir == '<') { var newX = (m - 2 + (b.x - 1) - 1) % (m - 2) + 1; b = (b.y, newX, b.dir); }
            if (b.dir == '>') { var newX = (b.x - 1 + 1) % (m - 2) + 1; b = (b.y, newX, b.dir); }
            if (b.dir == '^') { var newY = (n - 2 + (b.y - 1) - 1) % (n - 2) + 1; b = (newY, b.x, b.dir); }
            if (b.dir == 'v') { var newY = (b.y - 1 + 1) % (n - 2) + 1; b = (newY, b.x, b.dir); }

            blizzards[i] = b;
        }
    }
}