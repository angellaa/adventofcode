using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day23
{
    private readonly HashSet<(int y, int x)> elves = new();
    private readonly string[] directions = { "N", "S", "W", "E" };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var input = File.ReadAllLines("Input.txt").ToList();

        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '#')
                {
                    elves.Add((y, x));
                }
            }
        }
    }
    
    [Test]
    public void Part1()
    {
        for (var round = 0; round < 10; round++)
        {
            var moveIntents = new Dictionary<(int y, int x), (int newY, int newX)>();

            foreach (var (y, x) in elves)
            {
                moveIntents[(y, x)] = NextMove(round, y, x);
            }

            var moves = moveIntents.ToDictionary(x => x.Key, x => x.Value);
            var values = moveIntents.Keys.Select(x => moveIntents[x]).ToList();

            foreach (var elf in moveIntents.Keys)
            {
                if (values.Count(x => x == moveIntents[elf]) > 1)
                {
                    moves[elf] = elf;
                }
            }

            elves.Clear();

            foreach (var newElf in moves.Values)
            {
                elves.Add(newElf);
            }

            //Console.WriteLine($"Round {round+1}");
            //Print();
        }

        var minX = elves.Select(e => e.x).Min();
        var maxX = elves.Select(e => e.x).Max();
        var minY = elves.Select(e => e.y).Min();
        var maxY = elves.Select(e => e.y).Max();

        var count = 0;

        for (var y = minY; y <= maxY; y++)
        for (var x = minX; x <= maxX; x++)
        {
            if (!elves.Contains((y, x))) count++;
        }

        Assert.That(count, Is.EqualTo(110));
    }

    private (int, int) NextMove(int round, int y, int x)
    {
        if (!elves.Contains((y - 1, x)) && !elves.Contains((y - 1, x - 1)) && !elves.Contains((y - 1, x + 1)) &&
            !elves.Contains((y, x - 1)) && !elves.Contains((y, x + 1)) &&
            !elves.Contains((y + 1, x)) && !elves.Contains((y + 1, x - 1)) && !elves.Contains((y + 1, x + 1)))
        {
            return (y, x);
        }

        for (var i = 0; i < 4; i++)
        {
            var direction = directions[(round + i) % 4];

            switch (direction)
            {
                case "N" when !elves.Contains((y - 1, x)) && !elves.Contains((y - 1, x - 1)) && !elves.Contains((y - 1, x + 1)):
                    return (y - 1, x);

                case "S" when !elves.Contains((y + 1, x)) && !elves.Contains((y + 1, x - 1)) && !elves.Contains((y + 1, x + 1)):
                    return (y + 1, x);

                case "W" when !elves.Contains((y, x - 1)) && !elves.Contains((y - 1, x - 1)) && !elves.Contains((y + 1, x - 1)):
                    return (y, x - 1);

                case "E" when !elves.Contains((y, x + 1)) && !elves.Contains((y - 1, x + 1)) && !elves.Contains((y + 1, x + 1)):
                    return (y, x + 1);
            }
        }

        return (y, x);
    }

    void Print()
    {
        var minX = elves.Select(e => e.x).Min();
        var maxX = elves.Select(e => e.x).Max();
        var minY = elves.Select(e => e.y).Min();
        var maxY = elves.Select(e => e.y).Max();

        var count = 0;

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Console.Write(!elves.Contains((y, x)) ? "." : "#");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    [Test]
    public void Part2()
    {
        var lastElves = elves.ToHashSet();
        var round = 0;

        for (; round < 1000000; round++)
        {
            var moveIntents = new Dictionary<(int y, int x), (int newY, int newX)>();

            foreach (var (y, x) in elves)
            {
                moveIntents[(y, x)] = NextMove(round, y, x);
            }

            var moves = moveIntents.ToDictionary(x => x.Key, x => x.Value);
            var values = moveIntents.Keys.Select(x => moveIntents[x]).ToList();

            foreach (var elf in moveIntents.Keys)
            {
                if (values.Count(x => x == moveIntents[elf]) > 1)
                {
                    moves[elf] = elf;
                }
            }

            elves.Clear();

            foreach (var newElf in moves.Values)
            {
                elves.Add(newElf);
            }

            if (elves.SequenceEqual(lastElves))
            {
                break;
            }

            lastElves = elves.ToHashSet();
        }
        
        Assert.That(round + 1, Is.EqualTo(1008));
    }
}