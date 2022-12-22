using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day22
{
    private List<string> map = new();
    private List<string> instructions;
    private int n;
    private int m;
    private int x;
    private int y;
    private int dir;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var chunks = File.ReadAllLines("Input.txt").ChunkBy(x => x == "").ToList();

        map = chunks.First().ToList();
        n = map.Count;
        m = map[0].Length;

        for (var i = 0; i < n; i++)
        {
            if (map[i].Length < m) map[i] += new string(' ', m - map[i].Length);
        }

        instructions = chunks.Last().First().Replace("L", "L ").Replace("R", "R ").Split(" ").ToList();

        x = map[0].IndexOfAny(new[] { '.', '#' });
        y = 0;
        dir = 0; // right
    }

    [Test]
    public void Part1()
    {
        Console.WriteLine(y + " " + x + " " + dir);

        foreach (var instruction in instructions)
        {
            if (int.TryParse(instruction, out var steps))
            {
                Move(steps);
            }
            else
            {
                Move(int.Parse(instruction[..^1]));
                ChangeDirection(instruction[^1]);
            }

            Console.WriteLine(y + " " + x + " " + dir);
        }

        Assert.That(1000 * (y + 1) + 4 * (x + 1) + dir, Is.EqualTo(164014));
    }

    private void ChangeDirection(char c)
    {
        if (c == 'R') dir = (4 + dir + 1) % 4;
        if (c == 'L') dir = (4 + dir - 1) % 4;
    }

    private void Move(int steps)
    {
        for (var i = 0; i < steps; i++) Move();
    }

    private void Move()
    {
        if (dir == 0) // right
        {
            var newX = (x + m + 1) % m;

            if (map[y][newX] == ' ')
            {
                newX = map[y].IndexOfAny(new[] { '.', '#' });
            }

            if (map[y][newX] == '.') x = newX;
        }
        else if (dir == 1) // down
        {
            var newY = (y + n + 1) % n;

            if (map[newY][x] == ' ')
            {
                newY = new string(map.Select(l => l[x]).ToArray()).IndexOfAny(new[] { '.', '#' });
            }

            if (map[newY][x] == '.') y = newY;
        }
        else if (dir == 2) // left
        {
            var newX = (x + m - 1) % m;

            if (map[y][newX] == ' ')
            {
                newX = map[y].LastIndexOfAny(new[] { '.', '#' });
            }

            if (map[y][newX] == '.') x = newX;
        }
        else if (dir == 3) // up
        {
            var newY = (y + n - 1) % n;

            if (map[newY][x] == ' ')
            {
                newY = new string(map.Select(l => l[x]).ToArray()).LastIndexOfAny(new[] { '.', '#' });
            }

            if (map[newY][x] == '.') y = newY;
        }
    }

    [Test]
    public void Part2()
    {
    }

}