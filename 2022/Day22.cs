using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day22
{
    private List<string> map = new();
    private List<string>[] faces = { new(), new(), new(), new(), new(), new(), new() };
    private List<string> instructions;
    private int n;
    private int m;
    private int x;
    private int y;
    private int dir;
    private int face = 1;

    private readonly Dictionary<(int Face, int Dir), (int Face, int Dir)> mapping = new()
    {
        [(1, 3)] = (6, 2),
        [(1, 1)] = (3, 3),
        [(1, 2)] = (5, 2),
        [(1, 0)] = (2, 0),
        [(2, 3)] = (6, 1),
        [(2, 1)] = (3, 0),
        [(2, 2)] = (1, 0),
        [(2, 0)] = (4, 0),
        [(3, 3)] = (1, 1),
        [(3, 1)] = (4, 3),
        [(3, 2)] = (5, 3),
        [(3, 0)] = (2, 1),
        [(4, 3)] = (3, 1),
        [(4, 1)] = (6, 0),
        [(4, 2)] = (5, 0),
        [(4, 0)] = (2, 0),
        [(5, 3)] = (3, 2),
        [(5, 1)] = (6, 3),
        [(5, 2)] = (1, 2),
        [(5, 0)] = (4, 2),
        [(6, 3)] = (5, 1),
        [(6, 1)] = (2, 3),
        [(6, 2)] = (1, 3),
        [(6, 0)] = (4, 1)
    };

    [SetUp]
    public void SetUp()
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
        for (var i = 0; i < 50; i++)
        {
            faces[1].Add(map[i][50..100]);
            faces[2].Add(map[i][100..150]);
            faces[3].Add(map[i+50][50..100]);
            faces[4].Add(map[i+100][50..100]);
            faces[5].Add(map[i+100][..50]);
            faces[6].Add(map[i+150][..50]);
        }

        face = 1;
        x = 0;
        y = 0;

        Console.WriteLine($"({y},{x}) Dir={dir} Face={face}");

        foreach (var instruction in instructions)
        {
            if (int.TryParse(instruction, out var steps))
            {
                CubeMove(steps);
            }
            else
            {
                CubeMove(int.Parse(instruction[..^1]));
                ChangeDirection(instruction[^1]);
            }

            Console.WriteLine($"({y},{x}) Dir={dir} Face={face}");
        }

        Assert.That(1000 * (y + 1) + 4 * (x + 1) + dir, Is.EqualTo(0));
    }

    private void CubeMove(int steps)
    {
        for (var i = 0; i < steps; i++) CubeMove();
    }
    
    private void CubeMove()
    {
        var f = faces[face];
        int newX;
        int newY;

        if (dir == 0 && x < 49) // right
        {
            newX = x + 1;
            if (f[y][newX] == '.') x = newX;
            return;
        }
        
        if (dir == 1 && y < 49) // down
        {
            newY = y + 1;
            if (f[newY][x] == '.') y = newY;
            return;
        }

        if (dir == 2 && x > 0)  // left
        {
            newX = x - 1;
            if (f[y][newX] == '.') x = newX;
            return;
        }
        
        if (dir == 3 && y > 0) // up
        {
            newY = y - 1;
            if (f[newY][x] == '.') y = newY;
            return;
        }

        var (newFace, newDir) = mapping[(face, dir)];
        newX = newDir switch { 3 or 1 => x, 2 => 0, _ => 49 };
        newY = newDir switch { 2 or 0 => y, 3 => 0, _ => 49 };

        if (faces[newFace][newY][newX] == '.')
        {
            x = newX; y = newY; face = newFace;     
        }
    }
}