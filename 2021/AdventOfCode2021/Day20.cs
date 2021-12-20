using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day20
{
    private string alg;
    List<string> input;
    List<List<char>> map = new();
    private int n;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllLines("Day20.txt").ToList();

        alg = input[0];
        var initialMap = input.Skip(2).ToList();
        var n = initialMap.Count;

        for (var i = 0; i < 3 * n; i++)
            map.Add(new string('.', 3 * n).ToCharArray().ToList());

        for (var i = 0; i < n; i++)
            for (var j = 0; j < n; j++)
                map[n + i][n + j] = initialMap[i][j];

        this.n = 3 * n;

        Print();
    }

    private void Print()
    {
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                Console.Write(map[i][j]);
            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }

    [Test]
    public void Part1()
    {
        for (var k = 0; k < 50; k++)
        {
            var nextMap = new List<List<char>>();

            for (var i = 1; i < n - 1; i++)
            {
                var line = new List<char>();

                for (var j = 1; j < n - 1; j++)
                {
                    line.Add(Next(i, j));
                }

                nextMap.Add(line);
            }

            map = nextMap;
            n = map.Count;
            Print();
        }

        Assert.That(map.SelectMany(x => x).Count(x => x == '#'), Is.EqualTo(5275));
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }

    private char Next(int y, int x)
    {
        var s = $"{map[y-1][x-1]}{map[y-1][x]}{map[y-1][x+1]}{map[y][x-1]}{map[y][x]}{map[y][x+1]}{map[y+1][x-1]}{map[y+1][x]}{map[y+1][x+1]}";
        s = s.Replace(".", "0");
        s = s.Replace("#", "1");

        var index = Convert.ToInt32(s, 2);

        return alg[index];
    }
}