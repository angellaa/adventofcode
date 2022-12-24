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
    private int n;
    private int m;
    private const int maxMinute = 313;

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

        Assert.That(Count(), Is.EqualTo(301));
    }

    private int Count()
    {
        var root = (0, 1, 0);
        var explored = new HashSet<(int y, int x, int minute)> { root };

        var queue = new Queue<(int y, int x, int minute)>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var (y, x, minute) = queue.Dequeue();

            if (y == n - 1 && x == m - 2) return minute;

            (int y, int x, int minute) p;
                
            p = (y, x + 1, minute + 1);
            if (spacesByMinute[p.minute].Contains((p.y, p.x)) && !explored.Contains(p)) { explored.Add(p); queue.Enqueue(p); }

            p = (y, x - 1, minute + 1);
            if (spacesByMinute[p.minute].Contains((p.y, p.x)) && !explored.Contains(p)) { explored.Add(p); queue.Enqueue(p); }

            p = (y, x, minute + 1);
            if (spacesByMinute[p.minute].Contains((p.y, p.x)) && !explored.Contains(p)) { explored.Add(p); queue.Enqueue(p); }

            p = (y + 1, x, minute + 1);
            if (spacesByMinute[p.minute].Contains((p.y, p.x)) && !explored.Contains(p)) { explored.Add(p); queue.Enqueue(p); }

            p = (y - 1, x, minute + 1);
            if (spacesByMinute[p.minute].Contains((p.y, p.x)) && !explored.Contains(p)) { explored.Add(p); queue.Enqueue(p); }
        }

        return int.MaxValue;
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