using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day11
{
    List<List<int>> input;
    List<List<bool>> increased;
    private int n;
    private int m;


    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllLines("Day11.txt").Select(x => x.Select(y => (int)(y - '0')).ToList()).ToList();
        n = input.Count;
        m = input[0].Count;
    }

    [Test]
    public void Part1()
    {
        var result = 0;

        for (var step = 0; step < 100; step++)
        {
            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                input[i][j]++;
            }

            var flashes = 0;

            var toFlash = new List<(int, int)>();

            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                if (input[i][j] > 9)
                {
                    input[i][j] = 10;
                    toFlash.Add((i, j));
                }
            }

            do
            {
                var newToFlash = new List<(int, int)>();

                foreach (var (i, j) in toFlash)
                {
                    flashes++;

                    foreach (var (x, y) in Adj(i, j))
                    {
                        input[x][y]++;

                        if (input[x][y] == 10)
                        {
                            newToFlash.Add((x, y));
                        }
                    }
                }

                toFlash = newToFlash;

            } while (toFlash.Count > 0);

            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                if (input[i][j] >= 10)
                {
                    input[i][j] = 0;
                }
            }

            result += flashes;
        }

        Assert.That(result, Is.EqualTo(1656));
    }

    [Test]
    public void Part2()
    {
        var result = 0;
        var step = 0;

        for (step = 0; step < 1000; step++)
        {
            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                input[i][j]++;
            }

            var flashes = 0;

            var toFlash = new List<(int, int)>();

            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                if (input[i][j] > 9)
                {
                    input[i][j] = 10;
                    toFlash.Add((i, j));
                }
            }

            do
            {
                var newToFlash = new List<(int, int)>();

                foreach (var (i, j) in toFlash)
                {
                    flashes++;

                    foreach (var (x, y) in Adj(i, j))
                    {
                        input[x][y]++;

                        if (input[x][y] == 10)
                        {
                            newToFlash.Add((x, y));
                        }
                    }
                }

                toFlash = newToFlash;

            } while (toFlash.Count > 0);

            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                if (input[i][j] >= 10)
                {
                    input[i][j] = 0;
                }
            }

            if (input.SelectMany(x => x).Count(x => x == 0) == n * m)
            {
                break;
            }

            result += flashes;
        }

        Assert.That(step + 1, Is.EqualTo(1656));
    }

    string ToString(List<List<int>> map)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                sb.Append(map[i][j]);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    void Print()
    {
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                Console.Write(input[i][j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private IEnumerable<(int, int)> Adj(int r, int c)
    {
        for (int i = -1; i <= 1; i++)
        for (int j = -1; j <= 1; j++)
            if (InBounds(r + i, c + j)) 
                yield return (r + i, c + j);
    }

    private bool InBounds(int i, int j)
    {
        return i >= 0 && i < n && j >= 0 && j < m;
    }
}