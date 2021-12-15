using System.Reflection.Metadata.Ecma335;
using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day15
{
    List<List<int>> input;
    private int rows;
    private int cols;


    [SetUp]
    public void SetUp()
    {
        var lines = File.ReadAllLines("Day15.txt");
        rows = lines.Length;
        cols = lines[0].Length;
        input = lines.Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();
    }

    [Test]
    public void Part1()
    {
        var result = AStar(0, rows * cols - 1);

        Assert.That(result, Is.EqualTo(403));
    }
    
    [Test]
    public void Part2()
    {
        for (var r = 0; r < rows; r++)
        {
            var original = input[r].ToList();

            for (var i = 1; i < 5; i++)
            {
                input[r].AddRange(original.Select(x => 1 + (x - 1 + i) % 9));
            }
        }

        for (var i = 1; i < 5; i++)
        {
            for (var r = 0; r < rows; r++)
            {
                var original = input[r].ToList();

                input.Add(original.Select(x => 1 + (x - 1 + i) % 9).ToList());
            }
        }

        rows = input.Count;
        cols = input[0].Count;
        
        var result = AStar(0, rows * cols - 1);

        Assert.That(result, Is.EqualTo(2840));
    }

    int AStar(int start, int goal)
    {
        var openSet = new PriorityQueue<int, int>();
        openSet.Enqueue(start, 0);

        var gScore = new Dictionary<int, int>
        {
            [start] = 0
        };

        var fScore = new Dictionary<int, int>
        {
            [start] = h(start)
        };

        while (openSet.TryDequeue(out var current, out var priority))
        {
            if (current == goal)
            {
                return priority;
            }

            var neighbors = GetNeighbors(current).ToList();

            foreach (var neighbor in neighbors)
            {
                var tentative_gScore = g(current) + d(current, neighbor);

                if (tentative_gScore < g(neighbor))
                {
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = tentative_gScore + h(neighbor);

                    if (!openSet.UnorderedItems.Select(x => x.Element).Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, f(neighbor));
                    }
                }
            }
        }

        throw new Exception("failure");

        IEnumerable<int> GetNeighbors(int i)
        {
            var r = i / rows;
            var c = i % cols;
            
            if (InBound(r - 1, c)) yield return Index(r - 1, c);
            if (InBound(r + 1, c)) yield return Index(r + 1, c);
            if (InBound(r, c - 1)) yield return Index(r, c - 1);
            if (InBound(r, c + 1)) yield return Index(r, c + 1);
        }

        int g(int current) => gScore.ContainsKey(current) ? gScore[current] : int.MaxValue;
        int f(int current) => fScore.ContainsKey(current) ? fScore[current] : int.MaxValue;

        int h(int i)
        {
            var r = i / rows;
            var c = i % cols;
            
            return (rows - r) + (cols - c) - 2;
        }

        int d(int current, int neighbor) => Cost(neighbor);

        bool InBound(int r, int c) => r >= 0 && r < rows && c >=0 && c < cols;
    }

    private int Index(int r, int c) => r * rows + c;
    private int Cost(int i) => input[i / rows][i % cols];
    private int Cost(int r, int c) => Cost(Index(r, c));
}